#!/usr/bin/env bash
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT_FILE="$REPO_ROOT/src/H13y/H13y.csproj"
CHANGELOG_FILE="$REPO_ROOT/CHANGELOG.md"
ARTIFACTS_DIR="$REPO_ROOT/artifacts"
PACKAGE_ID="Humanizexternity"
BRANCH="main"

if [[ -t 1 ]]; then
    RED=$'\e[31m'; GREEN=$'\e[32m'; YELLOW=$'\e[33m'; BLUE=$'\e[34m'; BOLD=$'\e[1m'; RESET=$'\e[0m'
else
    RED=; GREEN=; YELLOW=; BLUE=; BOLD=; RESET=
fi
log()  { printf '%s==>%s %s\n' "$BLUE" "$RESET" "$*"; }
ok()   { printf '%s ok%s %s\n' "$GREEN" "$RESET" "$*"; }
warn() { printf '%swarn%s %s\n' "$YELLOW" "$RESET" "$*" >&2; }
die()  { printf '%serr%s %s\n' "$RED" "$RESET" "$*" >&2; exit 1; }

list_zip() {
    local file="$1"
    if command -v unzip >/dev/null 2>&1; then
        unzip -l "$file"
    elif command -v python3 >/dev/null 2>&1; then
        python3 -c 'import sys,zipfile; print("\n".join(zipfile.ZipFile(sys.argv[1]).namelist()))' "$file"
    else
        die "Need 'unzip' or 'python3' to inspect the .nupkg"
    fi
}

DRY_RUN=0; YES=0; SKIP_TESTS=0; VERSION_ARG=""
while [[ $# -gt 0 ]]; do
    case "$1" in
        --dry-run)    DRY_RUN=1; shift ;;
        -y|--yes)     YES=1; shift ;;
        --skip-tests) SKIP_TESTS=1; shift ;;
        -h|--help)
            cat <<EOF
Usage: scripts/release.sh <version> [options]

Creates a tagged release: builds, tests, packs, tags, and pushes.

Arguments:
  <version>       Semver, e.g. 1.3.2 or v1.3.2 (leading v is stripped)

Options:
  --dry-run       Run everything except tag creation and git push
  -y, --yes       Skip confirmation prompt
  --skip-tests    Skip dotnet test (not recommended)
  -h, --help      Show this help

Preconditions:
  - Working tree is clean (no staged/unstaged changes)
  - Currently on '$BRANCH'
  - csproj <Version> matches <version>
  - CHANGELOG.md contains a '## [<version>]' section
  - Tag v<version> does not exist locally or on origin
EOF
            exit 0 ;;
        -*) die "Unknown option: $1 (try --help)" ;;
        *)  VERSION_ARG="$1"; shift ;;
    esac
done

[[ -n "$VERSION_ARG" ]] || die "Missing version argument (try --help)"

VERSION="${VERSION_ARG#v}"
TAG="v$VERSION"

[[ "$VERSION" =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[0-9A-Za-z.-]+)?$ ]] \
    || die "Invalid version '$VERSION'. Expected MAJOR.MINOR.PATCH[-pre]."

log "Preparing release $TAG"
cd "$REPO_ROOT"

CURRENT_BRANCH="$(git rev-parse --abbrev-ref HEAD)"
[[ "$CURRENT_BRANCH" == "$BRANCH" ]] \
    || die "Not on '$BRANCH' (currently on '$CURRENT_BRANCH')."

if ! git diff --quiet || ! git diff --cached --quiet; then
    die "Working tree is dirty. Commit or stash first."
fi

git rev-parse --verify --quiet "refs/tags/$TAG" >/dev/null \
    && die "Local tag $TAG already exists."

if git ls-remote --tags --exit-code origin "refs/tags/$TAG" >/dev/null 2>&1; then
    die "Remote tag $TAG already exists on origin. NuGet may already have it."
fi

ok "Git state clean; $TAG is available"

CSPROJ_VERSION="$(sed -n 's/.*<Version>\([^<]*\)<\/Version>.*/\1/p' "$PROJECT_FILE" | head -1)"
[[ "$CSPROJ_VERSION" == "$VERSION" ]] \
    || die "csproj <Version> is '$CSPROJ_VERSION', expected '$VERSION'. Bump it first."
ok "csproj version matches: $CSPROJ_VERSION"

grep -q "^## \[$VERSION\]" "$CHANGELOG_FILE" \
    || die "CHANGELOG.md has no '## [$VERSION]' section."
ok "CHANGELOG.md has a $VERSION section"

if [[ $YES -eq 0 && $DRY_RUN -eq 0 ]]; then
    printf '\n%sRelease %s? This will tag and push.[%s] ' "$BOLD" "$TAG" "$RESET"
    read -r reply
    [[ "$reply" =~ ^[Yy]$ ]] || die "Aborted."
fi

log "dotnet restore"
dotnet restore "$REPO_ROOT"

log "dotnet build -c Release"
dotnet build --no-restore -c Release

if [[ $SKIP_TESTS -eq 1 ]]; then
    warn "Skipping tests (--skip-tests)"
else
    log "dotnet test -c Release"
    dotnet test --no-build -c Release --verbosity normal
fi

log "Cleaning $ARTIFACTS_DIR"
rm -rf "$ARTIFACTS_DIR"
mkdir -p "$ARTIFACTS_DIR"

log "dotnet pack -p:Version=$VERSION"
dotnet pack src/H13y/H13y.csproj --no-build -c Release -p:Version="$VERSION" -o "$ARTIFACTS_DIR"

NUPKG="$ARTIFACTS_DIR/$PACKAGE_ID.$VERSION.nupkg"
SNUPKG="$ARTIFACTS_DIR/$PACKAGE_ID.$VERSION.snupkg"

[[ -f "$NUPKG" ]]  || die "Package not found: $NUPKG"
[[ -f "$SNUPKG" ]] || warn "Symbols package not found: $SNUPKG"

if ! list_zip "$NUPKG" | grep -qi 'README\.md'; then
    die "README.md missing inside $NUPKG. Check the <None Include=\"../../README.md\" /> item in H13y.csproj."
fi
ok "Package created and contains README.md"

if [[ $DRY_RUN -eq 1 ]]; then
    warn "Dry run — skipping tag and push."
    ok "Artifacts ready at $ARTIFACTS_DIR"
    exit 0
fi

log "Pushing $BRANCH to origin"
git push origin "$BRANCH"

log "Creating annotated tag $TAG"
git tag -a "$TAG" -m "$TAG"

log "Pushing tag $TAG"
if ! git push origin "$TAG"; then
    warn "Tag push failed — deleting local tag to keep state consistent"
    git tag -d "$TAG"
    die "Rerun once the underlying issue is fixed."
fi

ok "Released $TAG"

if command -v gh >/dev/null 2>&1; then
    sleep 3
    RUN_ID="$(gh run list --workflow=publish.yml --limit 1 --json databaseId --jq '.[0].databaseId' 2>/dev/null || true)"
    if [[ -n "$RUN_ID" ]]; then
        log "Watching Publish workflow (Ctrl-C to stop watching; release continues regardless)"
        gh run watch "$RUN_ID" || true
    fi
fi

ok "Done."

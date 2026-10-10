#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CSPROJ="src/H13y/H13y.csproj"
CHANGELOG="CHANGELOG.md"
MAIN="main"
PKG="Humanizexternity"

if [[ -t 1 ]]; then
    R=$'\e[31m'; G=$'\e[32m'; Y=$'\e[33m'; B=$'\e[34m'; N=$'\e[0m'
else
    R=; G=; Y=; B=; N=
fi
log()  { printf '%s==>%s %s\n' "$B" "$N" "$*"; }
ok()   { printf '%s ok%s %s\n' "$G" "$N" "$*"; }
warn() { printf '%swarn%s %s\n' "$Y" "$N" "$*" >&2; }
die()  { printf '%serr%s %s\n' "$R" "$N" "$*" >&2; exit 1; }

DRY=0; YES=0; NOPUSH=0; NOWT=0; INFER=0; SKIPTEST=0
CLMODE="auto"
VER=""

while [[ $# -gt 0 ]]; do
    case "$1" in
        --dry-run)       DRY=1 ;;
        --no-push)       NOPUSH=1 ;;
        --no-worktree)   NOWT=1 ;;
        --infer)         INFER=1 ;;
        --skip-tests)    SKIPTEST=1 ;;
        --changelog=*)   CLMODE="${1#--changelog=}" ;;
        -y|--yes)        YES=1 ;;
        -h|--help)
            echo "usage: release.sh <version>|--infer [--dry-run] [--no-push] [--no-worktree] [--changelog=auto|manual|skip] [--skip-tests] [-y]"
            exit 0 ;;
        -*) die "unknown option: $1" ;;
        *)  VER="$1" ;;
    esac
    shift
done

[[ -n "$VER" || $INFER -eq 1 ]] || die "missing version or --infer"
[[ -z "$VER" || $INFER -eq 0 ]] || die "cannot combine version and --infer"
[[ "$CLMODE" == "auto" || "$CLMODE" == "manual" || "$CLMODE" == "skip" ]] || die "bad --changelog mode"

cd "$ROOT"

bump_major() { local v="${1#v}"; echo "$((${v%%.*} + 1)).0.0"; }
bump_minor() { local v="${1#v}"; local M="${v%%.*}"; local r="${v#*.}"; local m="${r%%.*}"; echo "$M.$((m + 1)).0"; }
bump_patch() { local v="${1#v}"; local M="${v%%.*}"; local r="${v#*.}"; local m="${r%%.*}"; local p="${r#*.}"; echo "$M.$m.$((p + 1))"; }

infer_bump() {
    local last_tag
    last_tag="$(git describe --tags --abbrev=0 2>/dev/null || true)"
    [[ -z "$last_tag" ]] && { echo "0.1.0"; return; }

    local log_range="$last_tag..HEAD"

    if git log "$log_range" --pretty=format:"%s" | grep -qE '^[a-z]+(\([^)]+\))?!:|^[a-z]+!:' \
       || git log "$log_range" --pretty=format:"%b" | grep -qE '^BREAKING[ -]CHANGE:'; then
        bump_major "$last_tag"; return
    fi
    if git log "$log_range" --pretty=format:"%s" | grep -qE '^feat(\([^)]+\))?:'; then
        bump_minor "$last_tag"; return
    fi
    if git log "$log_range" --pretty=format:"%s" | grep -qE '^fix(\([^)]+\))?:'; then
        bump_patch "$last_tag"; return
    fi
    die "no feat/fix/BREAKING commits since $last_tag"
}

read_ver() { sed -n 's/.*<Version>\([^<]*\)<\/Version>.*/\1/p' "$CSPROJ" | head -n 1; }
write_ver() { sed -i "s#<Version>[^<]*</Version>#<Version>$1</Version>#" "$CSPROJ"; }

has_section() { grep -qF "## [$1]" "$CHANGELOG"; }

prepend_section() {
    local tmp
    tmp="$(mktemp)"
    head -n 1 "$CHANGELOG" > "$tmp"
    printf '\n%s\n\n' "$1" >> "$tmp"
    tail -n +2 "$CHANGELOG" | sed '/./,$!d' >> "$tmp"
    mv "$tmp" "$CHANGELOG"
}

gen_section() {
    local ver="$1"
    local range="$2"
    local date
    date="$(date +%Y-%m-%d)"

    local added fixed changed perf msg
    added=""; fixed=""; changed=""; perf=""

    while IFS= read -r subject; do
        [[ -z "$subject" ]] && continue
        case "$subject" in
            feat:*|feat\(*)
                msg="${subject#*: }"
                [[ "$msg" == "$subject" ]] && msg="$(echo "$subject" | sed 's/^[^:]*: //')"
                added="$added- $msg
" ;;
            fix:*|fix\(*)
                msg="$(echo "$subject" | sed 's/^[^:]*: //')"
                fixed="$fixed- $msg
" ;;
            perf:*|perf\(*)
                msg="$(echo "$subject" | sed 's/^[^:]*: //')"
                perf="$perf- $msg
" ;;
            chore:*|docs:*|test:*|refactor:*|style:*|build:*|ci:*)
                : ;;
            *)
                changed="$changed- $subject
" ;;
        esac
    done < <(git log "$range" --pretty=format:"%s")

    printf '## [%s] - %s\n\n' "$ver" "$date"
    if [[ -n "$added" ]]; then
        printf '### Added\n%s\n' "$added"
    fi
    if [[ -n "$changed" ]]; then
        printf '### Changed\n%s\n' "$changed"
    fi
    if [[ -n "$fixed" ]]; then
        printf '### Fixed\n%s\n' "$fixed"
    fi
    if [[ -n "$perf" ]]; then
        printf '### Performance\n%s\n' "$perf"
    fi
}

WT=""
BR=""
cleanup() {
    if [[ -n "$WT" && -d "$WT" ]]; then
        git worktree remove --force "$WT" >/dev/null 2>&1 || true
    fi
    if [[ -n "$BR" ]] && git rev-parse --verify "$BR" >/dev/null 2>&1; then
        git branch -D "$BR" >/dev/null 2>&1 || true
    fi
}
trap cleanup EXIT

log "Preflight"

cur="$(git rev-parse --abbrev-ref HEAD)"
[[ "$cur" == "$MAIN" ]] || die "not on $MAIN (on $cur)"
if ! git diff --quiet || ! git diff --cached --quiet; then
    die "working tree is dirty"
fi

git fetch origin "$MAIN" >/dev/null 2>&1 || warn "could not fetch origin/$MAIN"
if git rev-parse --verify "origin/$MAIN" >/dev/null 2>&1; then
    L="$(git rev-parse HEAD)"
    Rm="$(git rev-parse "origin/$MAIN")"
    if [[ "$L" != "$Rm" ]]; then
        if git merge-base --is-ancestor "$Rm" "$L"; then
            :
        elif git merge-base --is-ancestor "$L" "$Rm"; then
            die "local $MAIN is behind origin/$MAIN (pull first)"
        else
            die "local $MAIN diverged from origin/$MAIN"
        fi
    fi
fi

LAST="$(git describe --tags --abbrev=0 2>/dev/null || true)"
ok "last tag: ${LAST:-<none>}"

if [[ $INFER -eq 1 ]]; then
    VER="$(infer_bump)"
    log "inferred version: $VER"
else
    VER="${VER#v}"
fi

[[ "$VER" =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[0-9A-Za-z.-]+)?$ ]] || die "invalid version '$VER'"
TAG="v$VER"

git rev-parse --verify --quiet "refs/tags/$TAG" >/dev/null && die "local tag $TAG exists"
if git ls-remote --tags origin "refs/tags/$TAG" 2>/dev/null | grep -q "$TAG"; then
    die "remote tag $TAG exists"
fi

COMMIT_RANGE="${LAST:+$LAST..HEAD}"
[[ -n "$COMMIT_RANGE" ]] || COMMIT_RANGE="HEAD"

ok "version $VER available"

if [[ $YES -eq 0 && $DRY -eq 0 ]]; then
    printf '\n%sRelease %s? tag + push.%s [y/N] ' "$B" "$TAG" "$N"
    read -r a
    [[ "$a" =~ ^[Yy]$ ]] || die "aborted"
fi

if [[ $NOWT -eq 1 ]]; then
    WORK="$ROOT"
    log "using current tree"
else
    WT="$(cd "$ROOT/.." && pwd)/h13y-release-$VER"
    BR="release/v$VER"
    if [[ -d "$WT" ]]; then
        git worktree remove --force "$WT" >/dev/null 2>&1 || true
        rm -rf "$WT" >/dev/null 2>&1 || true
    fi
    if git rev-parse --verify "$BR" >/dev/null 2>&1; then
        git branch -D "$BR" >/dev/null 2>&1 || true
    fi
    log "creating worktree at $WT"
    git worktree add -b "$BR" "$WT" HEAD >/dev/null
    WORK="$WT"
    ok "worktree ready"
fi

cd "$WORK"

cur_ver="$(read_ver)"
if [[ "$cur_ver" == "$VER" ]]; then
    ok "csproj already $VER"
else
    log "bump csproj: $cur_ver -> $VER"
    write_ver "$VER"
fi

if [[ "$CLMODE" == "skip" ]]; then
    warn "skipping CHANGELOG"
elif has_section "$VER"; then
    ok "CHANGELOG already has $VER"
elif [[ "$CLMODE" == "manual" ]]; then
    die "CHANGELOG.md has no $VER section"
else
    log "generating CHANGELOG section"
    sec="$(gen_section "$VER" "$COMMIT_RANGE")"
    prepend_section "$sec"
    ok "CHANGELOG updated"
fi

log "restore"
dotnet restore "$WORK" >/dev/null

log "build"
dotnet build --no-restore -c Release "$WORK"

if [[ $SKIPTEST -eq 1 ]]; then
    warn "skipping tests"
else
    log "test"
    dotnet test --no-build -c Release "$WORK" --verbosity normal
fi

rm -rf "$WORK/artifacts"
mkdir -p "$WORK/artifacts"

log "pack -p:Version=$VER"
dotnet pack src/H13y/H13y.csproj --no-build -c Release -p:Version="$VER" -o "$WORK/artifacts"

NUPKG="$WORK/artifacts/$PKG.$VER.nupkg"
[[ -f "$NUPKG" ]] || die "package not found: $NUPKG"

if ! unzip -l "$NUPKG" | grep -qi 'README\.md'; then
    die "README.md missing inside $NUPKG"
fi
ok "package contains README.md"

if [[ $DRY -eq 1 ]]; then
    warn "dry run — skipping commit/tag/push"
    ok "artifacts at $WORK/artifacts"
    exit 0
fi

log "commit release bump"
git add "$CSPROJ"
[[ "$CLMODE" == "skip" ]] || git add "$CHANGELOG"

if git diff --cached --quiet; then
    ok "nothing to commit"
else
    git commit -m "chore(release): bump version to $VER" \
               -m "Automated by scripts/release.sh."
fi

if [[ $NOWT -eq 0 ]]; then
    cd "$ROOT"
    log "fast-forward $MAIN"
    git merge --ff-only "$BR"
fi

cd "$ROOT"
log "create tag $TAG"
git tag -a "$TAG" -m "$TAG"

if [[ $NOPUSH -eq 1 ]]; then
    warn "skipping push. push manually:"
    printf '  git push origin %s\n  git push origin %s\n' "$MAIN" "$TAG"
    ok "local release ready: $TAG"
    exit 0
fi

log "push $MAIN"
git push origin "$MAIN"

log "push tag $TAG"
if ! git push origin "$TAG"; then
    warn "tag push failed — deleting local tag"
    git tag -d "$TAG" >/dev/null
    die "rerun once fixed"
fi

ok "released $TAG"

if command -v gh >/dev/null 2>&1; then
    sleep 3
    RUN_ID="$(gh run list --workflow=publish.yml --limit 1 --json databaseId --jq '.[0].databaseId' 2>/dev/null || true)"
    if [[ -n "$RUN_ID" ]]; then
        log "watching publish workflow (Ctrl-C to stop)"
        gh run watch "$RUN_ID" || true
    fi
fi

ok "done."

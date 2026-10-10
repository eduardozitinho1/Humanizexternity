# Changelog

## [1.8.0] - 2026-10-10

### Added
- add TemperatureDelta for temperature differences

## [1.7.0] - 2026-10-10

### Added
- add ParseDetailed for categorized parse failures
- add UnitRegistry for domain-specific custom units
- add H.Range, H.Uncertainty, and H.Between

## [1.6.0] - 2026-10-10

### Added
- `Dimension.DataRate` with a 1000-based bit chain (`bps`, `Kbps`, `Mbps`, `Gbps`, `Tbps`)
  and byte-per-second units (`B/s`, `KB/s`, `MB/s`, `GB/s`, `TB/s`) as the 8x
  counterparts. Typed `Measures.DataRate` record with `From`/`To` helpers,
  arithmetic, comparison, `IFormattable`, `Humanize`, and a JSON converter.
- `H.MegabitsPerSecond`, `H.GigabitsPerSecond`, `H.MegabytesPerSecond`, and
  `H.GigabytesPerSecond` facade methods.
- `H.DownloadDuration(DataSize, DataRate)` and `H.DownloadTime(DataSize, DataRate)`
  overloads accepting the new rate type directly.
- IEC binary bit units `Kibibit`, `Mebibit`, `Gibibit`, and `Tebibit` (1024-based)
  registered for parsing and conversion but excluded from auto-selection, so
  `"1 Kib"` parses as 1024 bits while `"1 Kb"` remains 1000 bits.
- `H.Approximate` for rough estimates: `about 1200`, `roughly 0.8`, `approximately
  2 million`. New `ApproximationPrecision` and `ApproximationStyle` enums, with
  localizable prefixes via `LocalizationKeys.Approximate*`.
- `H.TryParseCompact` to parse compact strings back to numbers: `"1.5M"` becomes
  `1_500_000`, `"1.5 million"` becomes `1_500_000`. Round-trips both styles.
- `H.TryParseOrdinal` to parse ordinals back to integers: `"22nd"` becomes 22,
  `"103rd"` becomes 103. Rejects mismatched suffixes like `"1th"`.
- `H.OrdinalWord` extended to cover values in `0..9999`: `"one thousandth"`,
  `"one thousand two hundred thirty-fourth"`.

### Changed
- `H.OrdinalWord` now accepts values up to 9999 instead of 999.

## [1.5.0] - 2026-10-09

### Added
- `Dimension.Bits` with 1000-based unit chain (`b`, `Kb`, `Mb`, `Gb`, `Tb`) and typed
  `Measures.Bits` record for network bandwidth, distinct from `DataSize` (bytes).
- `H.Compact` and `H.CompactWords` for humanizing raw numbers outside any measure
  dimension (`1.2K`, `1.5M`, `1.5 million`). New `CompactStyle` enum.
- `H.Ordinal` (`1st`, `22nd`) and `H.OrdinalWord` (`first`, `twenty-first`).
- `H.Roman`, `H.ParseRoman`, `H.TryParseRoman` with `RomanStyle.Subtractive`
  (`MMXXIV`) and `RomanStyle.Additive` (`IIII`).
- `H.DownloadDuration` and `H.DownloadTime` computing transfer time from a
  `DataSize` and a `Bits` rate.

## [1.4.0] - 2026-10-09

### Fixed
- Full-name output now round-trips through the parser. Multi-word labels
  (`"cubic meter"`, `"kilometer per hour"`, `"kilowatt-hour"`) are accepted,
  and `UnitAliases.Resolve` normalizes spaces, hyphens, and underscores before
  lookup, so every name emitted under `UnitStyle.FullName` parses back.
- Added missing aliases: `hectare`, `degree(s) Celsius`, `degree(s) Fahrenheit`,
  `mile per hour`, `foot per second`, `knot`, and singular `pound per square inch`.

### Tests
- Add `FullNameRoundTripTests`, which iterates every registered unit and verifies
  that the humanized string parses back to the same base value.

## [1.3.1] - 2026-10-09

### Fixed
- Restore the `<None Include="../../README.md" />` item removed from
  `H13y.csproj` in 1.3.0. Without it, `dotnet pack` failed with NU5039
  because the packaged README declared by `<PackageReadmeFile>` could not
  be found.

## [1.3.0] - 2026-10-09

### Added
- Localization through `Microsoft.Extensions.Localization.Abstractions`.
  `HumanizeOptions.Localizer` accepts an `IStringLocalizer` whose keys follow
  `LocalizationKeys` (`Unit_{symbol}_Singular` / `_Plural`,
  `RelativeTime_Now` / `_Moment` / `_Ago` / `_In`).
- Relative-time phrases go through the localizer, including the
  `{0} ago` / `in {0}` templates, so sentence order can be adapted per language.
- New dependency: `Microsoft.Extensions.Localization.Abstractions` (interfaces only).

### Changed
- `UnitNames.Resolve` (internal) gained an `IStringLocalizer?` parameter.
- Missing keys fall back to the built-in English strings.

## [1.2.0] - 2026-10-09

### Added
- `UnitStyle` enum (`Symbol`, `FullName`) and `HumanizeOptions.UnitStyle` to control
  whether unit labels are rendered as compact symbols (`"kg"`) or full English names
  (`"kilogram"`).
- `HumanizeOptions.Pluralize` (default `true`) to enable or disable pluralization
  when using `UnitStyle.FullName`.
- `H.RelativeTime(DateTime | DateTimeOffset, now?, options?)` for relative-time
  phrasing such as `"5 minutes ago"`, `"in 2 hours"`, and `"just now"`.

### Changed
- Default output is unchanged; `UnitStyle.Symbol` remains the default.

## [v1.1.0] – 2026-10-08

### Added
- Scientific notation support for single-value measurements, including
  positive and negative exponents and culture-specific decimal separators.

## [1.0.1] - 2026-10-08

### Fixed
- Return false from UnitParser.TryParse when text or culture is null.
- Reject colon-formatted durations with minutes or seconds outside the valid range.

### Tests
- Add regression tests for null arguments and invalid colon durations.

## [1.0.0] - 2026-10-04

### Changed (breaking)
- Every parsing API now requires an explicit `CultureInfo`: `H.Parse`, `H.ParseTimeSpan`,
  `H.TryParseTimeSpan`, all `H.TryParseXxx`, `H.Convert(string, Unit, CultureInfo)`,
  and `UnitParser.Parse` / `UnitParser.TryParse`. The parser respects the culture's
  decimal separator and rejects the other one, so `"1,5 GB"` only parses under a
  comma culture and `"1.5 GB"` only under a dot culture.
- `DecimalMode.Metric` renamed to `DecimalMode.Si`.

### Migration
- Add `using System.Globalization;` and pass `CultureInfo.InvariantCulture` where you
  previously had no culture. Use `CultureInfo.GetCultureInfo("pt-BR")` (or another
  comma culture) for inputs that use a comma as the decimal separator.
- Replace `DecimalMode.Metric` with `DecimalMode.Si`.

## [0.6.0] - 2026-10-04

### Changed
- Unit lookup by dimension is now a precomputed dictionary, so `Units.ByDimension` is allocation-free.
- `BestUnitSelector` caches per-dimension candidate lists sorted by descending factor.
- The single-value parser no longer uses a regex. It scans the input with `Span<char>`
  and `stackalloc`, keeping compound and colon duration parsing on compiled regexes.

### Added
- `<IsAotCompatible>true</IsAotCompatible>` on the package, so Native AOT consumers get
  analyzer coverage and full trimming support.
- `benchmarks/H13y.Benchmarks` project (BenchmarkDotNet) with humanize, parse, and
  convert benchmarks.

## [0.5.0] - 2026-10-04

### Added
- Speed dimension: m/s, km/h, mph, knot, ft/s.
- Energy dimension: J, kJ, MJ, cal, kcal, Wh, kWh.
- Power dimension: W, kW, MW, GW, hp.
- Pressure dimension: Pa, kPa, MPa, bar, psi, atm.
- Frequency dimension: Hz, kHz, MHz, GHz.
- Angle dimension: rad, deg, grad, turn.

## [0.4.0] - 2026-10-04

### Added
- IEC binary symbols, explicit conversion, IFormattable, TimeSpan integration,
  typed TryParse.

## [0.3.0] - 2026-10-04

### Added
- Temperature dimension.

## [0.2.1] - 2026-10-04

### Fixed
- Compound durations, NaN, infinity, zero in base unit.

## [0.2.0] - 2026-10-04

### Added
- Volume, Area, arithmetic, JSON converters.

## [0.1.0] - 2026-10-04

### Added
- Initial release.

# Changelog

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

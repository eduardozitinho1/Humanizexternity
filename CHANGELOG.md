# Changelog

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

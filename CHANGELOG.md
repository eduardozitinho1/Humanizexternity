# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.5.0] - 2026-10-04

### Added
- `Speed` dimension: m/s, km/h, mph, knot, ft/s.
- `Energy` dimension: J, kJ, MJ, cal, kcal, Wh, kWh.
- `Power` dimension: W, kW, MW, GW, hp.
- `Pressure` dimension: Pa, kPa, MPa, bar, psi, atm.
- `Frequency` dimension: Hz, kHz, MHz, GHz.
- `Angle` dimension: rad, deg, grad, turn.
- `H.MetersPerSecond`, `H.KilometersPerHour`, `H.MilesPerHour`.
- `H.Joules`, `H.Kilojoules`, `H.KilowattHours`, `H.Kilocalories`.
- `H.Watts`, `H.Kilowatts`, `H.Megawatts`, `H.Horsepower`.
- `H.Pascals`, `H.Kilopascals`, `H.Bars`, `H.Psi`.
- `H.Hertz`, `H.Kilohertz`, `H.Megahertz`, `H.Gigahertz`.
- `H.Radians`, `H.Degrees`.
- Degree symbol parsing: `"45°"`, `"180°"`.

### Changed
- Auto unit selection for speed, energy, pressure, and angle now restricts the
  candidates so output reads naturally (km/h, kWh, bar, deg) instead of picking
  the largest factor that fits.

## [0.4.0] - 2026-10-04

### Added
- IEC binary symbols (KiB, MiB, GiB, TiB, PiB) via `HumanizeOptions.UseIecSymbols`.
- Explicit unit conversion via `H.Convert`.
- `IFormattable` on every typed measure.
- `TimeSpan` integration: `H.TimeSpan(span)`, `H.ParseTimeSpan(text)`.
- Typed `TryParse` overloads per dimension.
- Round-trip, conversion, and formattable test suites.

## [0.3.0] - 2026-10-04

### Added
- Temperature dimension with affine conversions (°C, °F, K).
- `TemperatureScale` for explicit scale selection.
- `TemperatureFormatter`.

## [0.2.1] - 2026-10-04

### Fixed
- Parser accepts compound durations (`"1h30min"`) and colon separators (`"1:30"`).
- NaN, infinity, and zero handled gracefully.
- Zero renders in the base unit.

## [0.2.0] - 2026-10-04

### Added
- Volume and Area dimensions.
- Arithmetic operators on typed measures.
- System.Text.Json converters.

## [0.1.0] - 2026-10-04

### Added
- Initial release with Data, Mass, Length, and Time dimensions.

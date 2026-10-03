# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0] - 2026-10-03

### Added
- Initial release.
- `DataSize`, `Mass`, `Length`, `Duration` typed measures.
- Humanization for Data (B, KB, MB, GB, TB, PB), Mass (mg, g, kg, t),
  Length (mm, cm, m, km), and Time (ms, s, min, h, d, w).
- `H.Bytes`, `H.Grams`, `H.Meters`, `H.Seconds` facade methods.
- `H.Best(value, dimension)` auto unit selection.
- `H.Parse(string)` reverse parsing.
- `HumanizeOptions` for customizing output.

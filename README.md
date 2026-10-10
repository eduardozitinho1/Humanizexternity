# Humanizexternity (H13y)

[![NuGet](https://img.shields.io/nuget/v/Humanizexternity.svg)](https://www.nuget.org/packages/Humanizexternity)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Humanizexternity.svg)](https://www.nuget.org/packages/Humanizexternity)
[![CI](https://github.com/eduardozitinho1/Humanizexternity/actions/workflows/ci.yml/badge.svg)](https://github.com/eduardozitinho1/Humanizexternity/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Humanize any measure. `1024 MB` → `1 GB`. `1500 g` → `1.5 kg`. `3661 s` → `1 h 1 min 1 s`. `120 km/h` stays `120 km/h`. `3.6 MJ` → `1 kWh`.

A small C# library for turning raw numeric values into compact, human-readable strings, and for parsing those strings back into structured values. Built for .NET 10.

## Install

```
dotnet add package Humanizexternity
```

## Quick start

```csharp
using H13y;

H.Bytes(1073741824);              // "1 GB"
H.Grams(1500);                    // "1.5 kg"
H.Meters(0.005);                  // "5 mm"
H.Seconds(3661);                  // "1 h 1 min 1 s"
H.KilometersPerHour(100);         // "100 km/h"
H.Kilojoules(1.5);                // "1.5 kJ"
H.Kilowatts(1.5);                 // "1.5 kW"
H.Bars(1.5);                      // "1.5 bar"
H.Gigahertz(2.4);                 // "2.4 GHz"
H.Degrees(45);                    // "45 deg"
H.Bits(1_000_000_000);            // "1 Gb"
H.Compact(1_500_000);             // "1.5M"
H.CompactWords(1_500_000);        // "1.5 million"
H.Ordinal(22);                    // "22nd"
H.OrdinalWord(22);                // "twenty-second"
H.Roman(2024);                    // "MMXXIV"
```

## Supported dimensions

| Dimension    | Base unit | Units                       |
|--------------|-----------|-----------------------------|
| Data         | byte      | B, KB, MB, GB, TB, PB       |
| Mass         | gram      | mg, g, kg, t                |
| Length       | meter     | mm, cm, m, km               |
| Time         | second    | ms, s, min, h, d, w         |
| Volume       | liter     | ml, cl, l, m3               |
| Area         | m²        | mm2, cm2, m2, ha, km2       |
| Temperature  | kelvin    | °C, °F, K                   |
| Speed        | m/s       | m/s, km/h, mph, kn, ft/s    |
| Energy       | joule     | J, kJ, MJ, cal, kcal, Wh, kWh |
| Power        | watt      | W, kW, MW, GW, hp           |
| Pressure     | pascal    | Pa, kPa, MPa, bar, psi, atm |
| Frequency    | hertz     | Hz, kHz, MHz, GHz           |
| Angle        | radian    | rad, deg, grad, turn        |
| Bits         | bit       | b, Kb, Mb, Gb, Tb           |

Notes:

- Auto unit selection picks the largest unit that fits. A few dimensions restrict the candidates so output reads naturally: speed shows km/h, energy shows J/kJ/MJ/kWh, pressure shows Pa/kPa/MPa/bar, angle shows deg. The other units stay available via the parser and `H.Convert`.
- Data uses 1024-based scaling by default. Set `DecimalMode.Metric` for SI-style 1000-based scaling, and `UseIecSymbols = true` for KiB/MiB/GiB.
- Temperature is affine, so `H.Best` rejects `Dimension.Temperature`. Use `H.Celsius`, `H.Fahrenheit`, or `H.Kelvin`.
- Zero always renders in the base unit: `Mass.FromGrams(0)` prints `"0 g"`, not `"0 mg"`.

## Typed measures

Each dimension has a `readonly record struct` under `H13y.Measures`.

```csharp
using H13y.Measures;

var size = DataSize.FromMegabytes(1024);
size.Humanize();                  // "1 GB"
size.ToGigabytes();               // 1.0

var energy = Energy.FromKilowattHours(1);
energy.ToMegajoules();            // 3.6
energy.Humanize();                // "1 kWh"

var angle = Angle.FromDegrees(180);
angle.ToRadians();                // ~3.14159
```

## Parsing

```csharp
var m = H.Parse("1.5 kg");
m.Value;                // 1.5
m.Unit.Symbol;          // "kg"
m.ToBase();             // 1500

H.Parse("100 km/h").ToBase();   // 27.77... (m/s)
H.Parse("1 kWh").ToBase();      // 3_600_000 (J)
H.Parse("45°").ToBase();        // 0.785... (rad)
```

Accepted forms:

- Optional space between value and unit: `"1GB"`, `"1 GB"`.
- Case-insensitive symbols: `"gb"`, `"GB"`, `"Gb"`.
- Full names and aliases: `"kilogram"`, `"kilos"`, `"knots"`, `"kilowatthour"`.
- Comma or dot as decimal separator: `"1,5"`, `"1.5"`.
- Compound durations: `"1h30min"`, `"1 h 30 min"`, `"1h30m45s"`.
- Colon durations: `"1:30"` (mm:ss), `"1:30:45"` (hh:mm:ss).
- Degree symbol: `"45°"`, `"180°"`.

## Arithmetic and comparison

Every typed measure supports `+`, `-`, `<`, `<=`, `>`, `>=`, `==`, `!=`.

```csharp
var a = Mass.FromKilograms(1.5);
var b = Mass.FromGrams(500);
var total = a + b;                // 2 kg

var p1 = Power.FromKilowatts(1);
var p2 = Power.FromWatts(500);
var sum = p1 + p2;                // 1.5 kW
```

## JSON serialization

```csharp
using System.Text.Json;
using H13y.Json;

var json = JsonSerializer.Serialize(Mass.FromKilograms(1.5), H13yJson.Options);
// "1500"

var m = H.Parse("1.5 kg");
var obj = JsonSerializer.Serialize(m, H13yJson.Options);
// {"value":1.5,"unit":"kg"}
```

## Options

```csharp
var opts = new HumanizeOptions
{
    Mode = DecimalMode.Binary,               // 1024-based (default) or Metric
    Culture = CultureInfo.InvariantCulture,  // dot decimal separator (default)
    MaxDecimals = 2,
    SpaceBetweenValueAndUnit = true,
    UseIecSymbols = false,                   // KiB/MiB/GiB instead of KB/MB/GB
};

H.Bytes(1536, opts);                                  // "1.5 KB"
H.Bytes(1536, opts with { UseIecSymbols = true });    // "1.5 KiB"
```

## Full unit names and pluralization

```csharp
var full = new HumanizeOptions { UnitStyle = UnitStyle.FullName };

H.Grams(1, full);              // "1 gram"
H.Grams(1.5, full);            // "1.5 kilograms"
H.Bytes(1073741824, full);     // "1 gigabyte"
H.Bytes(2147483648, full);     // "2 gigabytes"
H.Seconds(3661, full);         // "1 hour 1 minute 1 second"
H.Celsius(25, full);           // "25 degrees Celsius"
```

`Pluralize = false` on `HumanizeOptions` disables the automatic plural.

## Relative time

```csharp
H.RelativeTime(DateTime.UtcNow.AddMinutes(-5));   // "5 minutes ago"
H.RelativeTime(DateTime.UtcNow.AddHours(2));      // "in 2 hours"
H.RelativeTime(DateTime.UtcNow.AddSeconds(-10));  // "just now"
H.RelativeTime(DateTime.UtcNow.AddDays(1));       // "in 1 day"
```

Pass a reference time as the second argument to make the output deterministic
(useful in tests). Both `DateTime` and `DateTimeOffset` overloads are available.

## Localization

The library ships built-in English strings. To translate unit names and
relative-time phrases, pass an `IStringLocalizer` (from
[`Microsoft.Extensions.Localization.Abstractions`](https://www.nuget.org/packages/Microsoft.Extensions.Localization.Abstractions))
via `HumanizeOptions.Localizer`.

Keys follow `LocalizationKeys`:

| Key | Meaning |
|---|---|
| `Unit_{symbol}_Singular` | Singular unit name (`Unit_kg_Singular` = "quilograma") |
| `Unit_{symbol}_Plural` | Plural unit name (`Unit_kg_Plural` = "quilogramas") |
| `RelativeTime_Now` | "just now" |
| `RelativeTime_Moment` | "in a moment" |
| `RelativeTime_Ago` | "{0} ago" — template with one argument |
| `RelativeTime_In` | "in {0}" — template with one argument |

Missing keys fall back to the built-in English strings, so translations can be
added incrementally.

```csharp
var opts = new HumanizeOptions
{
    UnitStyle = UnitStyle.FullName,
    Culture   = new CultureInfo("pt-BR"),
    Localizer = localizer, // your IStringLocalizer
};

H.Grams(1500, opts);                                            // "1,5 quilogramas"
H.Seconds(3661, opts);                                          // "1 hora 1 minuto 1 segundo"
H.RelativeTime(DateTime.UtcNow.AddMinutes(-5), options: opts);  // "há 5 minutos"
```

## IFormattable

```csharp
var size = DataSize.FromKilobytes(1536);
size.ToString("H", CultureInfo.InvariantCulture);     // "1.5 MB"
size.ToString("I", CultureInfo.InvariantCulture);     // "1.5 MiB"
size.ToString("R", CultureInfo.InvariantCulture);     // "1572864"
size.ToString("N2", CultureInfo.InvariantCulture);    // "1.50"
```

## Performance

The library is allocation-light and AOT-friendly.

- Unit lookup and best-unit selection are cached per dimension.
- The single-value parser scans with `Span<char>` and `stackalloc` instead of a regex.
- `<IsAotCompatible>true</IsAotCompatible>` is set on the package.

Run benchmarks locally:

```
dotnet run --project benchmarks/H13y.Benchmarks -c Release
```

The benchmark project is intentionally kept out of the solution so `dotnet build`
and `dotnet test` on the repo stay fast.

## License

MIT

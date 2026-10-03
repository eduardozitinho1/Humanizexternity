# Humanizexternity (H13y)

[![NuGet](https://img.shields.io/nuget/v/Humanizexternity.svg)](https://www.nuget.org/packages/Humanizexternity)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Humanizexternity.svg)](https://www.nuget.org/packages/Humanizexternity)
[![CI](https://github.com/eduardozitinho1/Humanizexternity/actions/workflows/ci.yml/badge.svg)](https://github.com/eduardozitinho1/Humanizexternity/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Humanize any measure. `1024 MB` → `1 GB`. `1500 g` → `1.5 kg`. `3661 s` → `1 h 1 min 1 s`. `500 ml` stays `500 ml`. `10000 m2` → `1 ha`.

A small, dependency-free C# library for turning raw numeric values into compact, human-readable strings, and for parsing those strings back into structured values. Built for .NET 10, with a clean API that works well with modern C# features and AI-generated code.

## Table of contents

- [Install](#install)
- [Quick start](#quick-start)
- [Supported dimensions](#supported-dimensions)
- [Typed measures](#typed-measures)
- [Auto unit selection](#auto-unit-selection)
- [Parsing](#parsing)
- [Arithmetic and comparison](#arithmetic-and-comparison)
- [JSON serialization](#json-serialization)
- [Options](#options)
- [Design principles](#design-principles)
- [Contributing](#contributing)
- [License](#license)

## Install

```
dotnet add package Humanizexternity
```

The package targets .NET 10.0 and has no external dependencies.

## Quick start

```csharp
using H13y;

H.Bytes(1073741824);              // "1 GB"
H.Bytes(1536);                    // "1.5 KB"
H.Grams(1500);                    // "1.5 kg"
H.Meters(0.005);                  // "5 mm"
H.Seconds(3661);                  // "1 h 1 min 1 s"
H.Liters(1.5);                    // "1.5 l"
H.SquareMeters(10_000);           // "1 ha"
```

Every method is a thin wrapper over a strongly typed measure. If you need conversions, arithmetic, or serialization, use the typed records directly.

## Supported dimensions

| Dimension | Base unit | Units |
|-----------|-----------|-------|
| Data      | byte      | B, KB, MB, GB, TB, PB |
| Mass      | gram      | mg, g, kg, t |
| Length    | meter     | mm, cm, m, km |
| Time      | second    | ms, s, min, h, d, w |
| Volume    | liter     | ml, cl, l, m3 |
| Area      | m²        | mm2, cm2, m2, ha, km2 |

Notes:

- Data uses 1024-based scaling by default (`1 KB = 1024 B`). Set `DecimalMode.Metric` for SI-style 1000-based scaling.
- Time is formatted with compound notation (`"1 h 30 min 5 s"`) because time units are not powers of ten.
- Centiliter (`cl`) is accepted by the parser and available via `Volume.FromCentiliters`, but it is not picked by auto unit selection. Everyday output prefers `500 ml` over `50 cl`.

## Typed measures

Each dimension has a strongly typed `readonly record struct` under `H13y.Measures`. These are immutable, value-based, and cheap to copy.

```csharp
using H13y.Measures;

var size = DataSize.FromMegabytes(1024);
size.Humanize();                  // "1 GB"
size.ToGigabytes();               // 1.0
size.Bytes;                       // 1073741824

var mass = Mass.FromKilograms(1.5);
mass.Humanize();                  // "1.5 kg"
mass.ToGrams();                   // 1500

var length = Length.FromMeters(1500);
length.Humanize();                // "1.5 km"

var duration = Duration.FromHours(1.5);
duration.Humanize();              // "1 h 30 min"

var volume = Volume.FromLiters(1.5);
volume.Humanize();                // "1.5 l"

var area = Area.FromHectares(1);
area.Humanize();                  // "1 ha"
```

Every type follows the same pattern: `From*` factories, `To*` converters, and a `Humanize` method for display.

## Auto unit selection

When you have a value in the dimension's base unit but don't know which typed record to use, call `H.Best`:

```csharp
H.Best(1073741824, Dimension.Data);   // "1 GB"
H.Best(1500, Dimension.Mass);         // "1.5 kg"
H.Best(0.005, Dimension.Length);      // "5 mm"
H.Best(3661, Dimension.Time);         // "1 h 1 min 1 s"
H.Best(1.5, Dimension.Volume);        // "1.5 l"
H.Best(10_000, Dimension.Area);       // "1 ha"
```

The selector walks the units of the dimension from largest to smallest and returns the first one whose factor fits. Values smaller than the smallest unit fall back to the smallest unit, so nothing throws for perfectly valid small numbers.

## Parsing

`H.Parse` reads a human-readable string into a `Measure`, which carries the original value and unit.

```csharp
var m = H.Parse("1.5 kg");
m.Value;                // 1.5
m.Unit.Symbol;          // "kg"
m.ToBase();             // 1500  (grams, the base unit of Mass)

H.Parse("500 ml").ToBase();      // 0.5    (liters)
H.Parse("1 m3").ToBase();        // 1000   (liters)
H.Parse("1,5 GB").ToBase();      // 1.5e9  (bytes, comma accepted as decimal separator)
```

Accepted forms:

- Optional space between value and unit: `"1GB"`, `"1 GB"`.
- Case-insensitive symbols: `"gb"`, `"GB"`, `"Gb"`.
- Full names and aliases: `"kilogram"`, `"kilos"`, `"hr"`, `"squaremeter"`.
- Comma or dot as decimal separator: `"1,5"`, `"1.5"`.

`H.TryParse` is also available if you prefer a boolean over an exception:

```csharp
if (UnitParser.TryParse("1.5 kg", out var measure))
{
    // use measure
}
```

Unknown unit symbols throw `FormatException` with a clear message. Use `TryParse` to handle failures gracefully.

## Arithmetic and comparison

Every typed measure supports `+`, `-`, `<`, `<=`, `>`, `>=`, `==`, and `!=`. Operations keep the values in their base unit and never lose precision.

```csharp
var a = Mass.FromKilograms(1.5);
var b = Mass.FromGrams(500);

var total = a + b;                // 2 kg
var diff  = a - b;                // 1 kg
bool bigger = a > b;              // true
bool same   = a == Mass.FromGrams(1500);  // true
```

The same pattern works for every dimension:

```csharp
var total = DataSize.FromMegabytes(500) + DataSize.FromMegabytes(700);  // 1200 MB
var span  = Duration.FromHours(2) - Duration.FromMinutes(30);           // 1 h 30 min
var plot  = Area.FromHectares(1) + Area.FromSquareMeters(5000);         // 1.5 ha
```

## JSON serialization

Two serialization shapes are available through `System.Text.Json`.

**Typed measures** serialize as a single number in their base unit, which produces compact payloads:

```csharp
using System.Text.Json;
using H13y.Json;
using H13y.Measures;

var json = JsonSerializer.Serialize(Mass.FromKilograms(1.5), H13yJson.Options);
// "1500"

var mass = JsonSerializer.Deserialize<Mass>("1500", H13yJson.Options);
// Mass { Grams = 1500 }
```

**Measure** serializes as an object with the value and unit, which is useful when the unit matters:

```csharp
var m = H.Parse("1.5 kg");
var json = JsonSerializer.Serialize(m, H13yJson.Options);
// {"value":1.5,"unit":"kg"}

var restored = JsonSerializer.Deserialize<Measure>(json, H13yJson.Options);
restored.Value;         // 1.5
restored.Unit.Symbol;   // "kg"
```

`H13yJson.Options` is a shared, thread-safe `JsonSerializerOptions` with every converter registered. You can also register converters individually if you already have your own options instance.

## Options

`HumanizeOptions` controls the output format.

```csharp
var opts = new HumanizeOptions
{
    Mode = DecimalMode.Binary,               // 1024-based (default) or Metric (1000-based)
    Culture = CultureInfo.InvariantCulture,  // dot decimal separator (default)
    MaxDecimals = 2,                         // up to 2 decimals, trailing zeros omitted
    SpaceBetweenValueAndUnit = true,         // "1 GB" (default) or "1GB"
};

H.Bytes(1536, opts);                                  // "1.5 KB"
H.Bytes(1536, opts with { SpaceBetweenValueAndUnit = false });  // "1.5KB"

var ptBr = opts with { Culture = new CultureInfo("pt-BR") };
H.Bytes(1536, ptBr);                                  // "1,5 KB"
```

`HumanizeOptions` is an immutable record, so you can derive variations with `with` expressions without affecting the shared default.

## Design principles

- **No external dependencies.** The entire library builds on the BCL. It stays fast to restore, easy to audit, and safe for AOT scenarios.
- **Immutable value types.** Every measure is a `readonly record struct`, which makes them thread-safe, cheap to copy, and easy to use in collections and dictionaries.
- **One base unit per dimension.** All conversions go through the base unit (byte, gram, meter, second, liter, square meter). This keeps arithmetic exact and makes reasoning about values straightforward.
- **Predictable output.** Humanization always prefers the largest unit that fits, never produces trailing zeros, and never throws for valid positive values.
- **Parsing that respects users.** Both comma and dot decimal separators are accepted, units are case-insensitive, and both symbols and full names work as input.

## Contributing

Issues and pull requests are welcome. The project follows Conventional Commits and Semantic Versioning.

Local setup:

```
git clone https://github.com/eduardozitinho1/Humanizexternity
cd Humanizexternity
dotnet build
dotnet test
```

Tests use xUnit. New features should come with tests covering the happy path and at least one edge case.

## License

MIT. See [LICENSE](LICENSE) for the full text.

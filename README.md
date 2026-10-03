# Humanizexternity (H13y)

Humanize any measure. `1024 MB` → `1 GB`. `1500 g` → `1.5 kg`. `3661 s` → `1 h 1 min 1 s`.

## Install

```bash
dotnet add package Humanizexternity
```

## Usage

```csharp
using H13y;

H.Bytes(1073741824);           // "1 GB"
H.Bytes(1536);                 // "1.5 KB"
H.Grams(1500);                 // "1.5 kg"
H.Meters(0.005);               // "5 mm"
H.Seconds(3661);               // "1 h 1 min 1 s"

H.Best(1500, Dimension.Mass);  // "1.5 kg"

var m = H.Parse("1.5 kg");
m.Value;                        // 1.5
m.Unit.Symbol;                  // "kg"

```

## Typed measures

```csharp
using H13y.Measures;

var size = DataSize.FromMegabytes(1024);
size.Humanize();               // "1 GB"
size.ToGigabytes();            // 1.0

var mass = Mass.FromKilograms(1.5);
mass.Humanize();               // "1.5 kg"
mass.ToGrams();                // 1500

var length = Length.FromMeters(1500);
length.Humanize();             // "1.5 km"

var duration = Duration.FromHours(1.5);
duration.Humanize();           // "1 h 30 min"
```

## Options

```csharp
var opts = new HumanizeOptions
{
    MaxDecimals = 2,
    SpaceBetweenValueAndUnit = false,
};
H.Bytes(1536, opts);           // "1.5KB"
```

## Dimensions

- **Data**: B, KB, MB, GB, TB, PB
- **Mass**: mg, g, kg, t
- **Length**: mm, cm, m, km
- **Time**: ms, s, min, h, d, w

## License

MIT – See full license at [LICENSE](LICENSE)


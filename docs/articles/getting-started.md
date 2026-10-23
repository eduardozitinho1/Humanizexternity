# Getting started

Humanizexternity groups measures into **dimensions** — mass, length, time, data, temperature, and so on. Each dimension has a base unit (gram for mass, meter for length, second for time) and the library picks the best unit for display automatically.

## Choosing the unit

Use `H.Best` when you want to control the dimension explicitly:

    H.Best(1500, Dimension.Mass);   // "1.5 kg"

For common dimensions, the facade methods are shorter:

    H.Grams(1500);                  // "1.5 kg"
    H.Meters(1500);                 // "1.5 km"
    H.Bytes(1073741824);            // "1 GB"

## Parsing strings

The parser is culture-aware and requires an explicit `CultureInfo` since v1.0.0. The culture determines the decimal separator: a comma culture parses `"1,5 kg"` and rejects `"1.5 kg"`; a dot culture does the opposite.

    using System.Globalization;

    H.Parse("1,5 kg", new CultureInfo("pt-BR")).ToBase();   // 1500
    H.Parse("1.5 kg", CultureInfo.InvariantCulture).ToBase(); // 1500

## Typed measures

Each dimension has a typed record struct under `H13y.Measures`. Arithmetic and comparison work across units within the same dimension:

    using H13y.Measures;

    var a = Mass.FromKilograms(1.5);
    var b = Mass.FromGrams(500);
    var total = a + b;              // 2 kg

    var size = DataSize.FromMegabytes(1024);
    size.Humanize();                // "1 GB"
    size.ToGigabytes();             // 1.0

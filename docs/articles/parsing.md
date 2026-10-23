# Parsing

`H.Parse` converts a human-readable string into a `Measure` — a value paired with a `Unit`. The result keeps the value exactly as typed; call `ToBase()` to convert it to the dimension's base unit.

    using System.Globalization;
    using H13y;

    var m = H.Parse("1.5 kg", CultureInfo.InvariantCulture);
    m.Value;              // 1.5
    m.Unit.Symbol;        // "kg"
    m.ToBase();           // 1500

## Accepted forms

- Optional space between value and unit: `"1GB"` and `"1 GB"` both work.
- Case-insensitive symbols: `"gb"`, `"GB"`, `"Gb"`.
- Full names and aliases: `"kilogram"`, `"kilos"`, `"knots"`.
- Culture-specific decimal separator: `"1,5"` under pt-BR, `"1.5"` under invariant.
- Compound durations: `"1h30min"`, `"1 h 30 min"`, `"1h30m45s"`.
- Colon durations: `"1:30"` (mm:ss), `"1:30:45"` (hh:mm:ss).

## Categorized failures

When you need to know why a parse failed, use `H.ParseDetailed`:

    var result = H.ParseDetailed("1 xyz", CultureInfo.InvariantCulture);

    if (!result.Success)
    {
        result.ErrorKind;    // ParseErrorKind.UnknownUnit
        result.ErrorMessage; // "Unknown unit 'xyz'."
    }

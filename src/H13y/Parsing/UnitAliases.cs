namespace H13y;

/// <summary>
/// Resolves unit symbols and their aliases to the registered <see cref="Unit"/> instances.
/// </summary>
/// <remarks>
/// Users type units in many different ways: "kg", "KG", "kilogram", "kilograms", "kilo",
/// "kilos". Rather than force a single canonical spelling, this class builds a lookup
/// dictionary at startup that maps every accepted form to the same <see cref="Unit"/>.
///
/// The lookup is case-insensitive, so "gb", "GB", and "Gb" all resolve to the same unit.
/// Both the symbol and the full English word are registered, plus a few common short forms
/// such as "hr" for hour. When you add a new unit to <see cref="Units"/>, remember to add
/// its aliases here so the parser can recognize user input.
/// </remarks>
internal static class UnitAliases
{
    private static readonly Dictionary<string, Unit> Map = BuildMap();

    /// <summary>
    /// Resolves a unit symbol or alias to its <see cref="Unit"/>, or returns <c>null</c> if unknown.
    /// </summary>
    /// <param name="symbol">The symbol or alias, such as "kg" or "kilogram".</param>
    /// <returns>The matching unit, or <c>null</c> when the input is empty or unrecognized.</returns>
    public static Unit? Resolve(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return null;

        return Map.TryGetValue(symbol.Trim(), out var unit) ? unit : null;
    }

    private static Dictionary<string, Unit> BuildMap()
    {
        var map = new Dictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);

        void Add(Unit unit, params string[] aliases)
        {
            map[unit.Symbol] = unit;
            foreach (var alias in aliases)
                map[alias] = unit;
        }

        Add(Units.Data.Byte, "byte", "bytes");
        Add(Units.Data.Kilobyte, "kilobyte", "kilobytes");
        Add(Units.Data.Megabyte, "megabyte", "megabytes");
        Add(Units.Data.Gigabyte, "gigabyte", "gigabytes");
        Add(Units.Data.Terabyte, "terabyte", "terabytes");
        Add(Units.Data.Petabyte, "petabyte", "petabytes");

        Add(Units.Mass.Milligram, "milligram", "milligrams");
        Add(Units.Mass.Gram, "gram", "grams");
        Add(Units.Mass.Kilogram, "kilogram", "kilograms", "kilo", "kilos");
        Add(Units.Mass.Tonne, "ton", "tons", "tonne", "tonnes");

        Add(Units.Length.Millimeter, "millimeter", "millimeters", "millimetre", "millimetres");
        Add(Units.Length.Centimeter, "centimeter", "centimeters", "centimetre", "centimetres");
        Add(Units.Length.Meter, "meter", "meters", "metre", "metres");
        Add(Units.Length.Kilometer, "kilometer", "kilometers", "kilometre", "kilometres");

        Add(Units.Time.Millisecond, "millisecond", "milliseconds");
        Add(Units.Time.Second, "sec", "secs", "second", "seconds");
        Add(Units.Time.Minute, "mins", "minute", "minutes");
        Add(Units.Time.Hour, "hr", "hrs", "hour", "hours");
        Add(Units.Time.Day, "days");
        Add(Units.Time.Week, "weeks");

        return map;
    }
}

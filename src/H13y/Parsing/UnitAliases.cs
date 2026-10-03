namespace H13y;

/// <summary>
/// Resolves unit symbols (with aliases) to <see cref="Unit"/> instances.
/// </summary>
internal static class UnitAliases
{
    private static readonly Dictionary<string, Unit> Map = BuildMap();

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

        // Data
        Add(Units.Data.Byte, "byte", "bytes");
        Add(Units.Data.Kilobyte, "kilobyte", "kilobytes");
        Add(Units.Data.Megabyte, "megabyte", "megabytes");
        Add(Units.Data.Gigabyte, "gigabyte", "gigabytes");
        Add(Units.Data.Terabyte, "terabyte", "terabytes");
        Add(Units.Data.Petabyte, "petabyte", "petabytes");

        // Mass
        Add(Units.Mass.Milligram, "milligram", "milligrams");
        Add(Units.Mass.Gram, "gram", "grams");
        Add(Units.Mass.Kilogram, "kilogram", "kilograms", "kilo", "kilos");
        Add(Units.Mass.Tonne, "ton", "tons", "tonne", "tonnes");

        // Length
        Add(Units.Length.Millimeter, "millimeter", "millimeters", "millimetre", "millimetres");
        Add(Units.Length.Centimeter, "centimeter", "centimeters", "centimetre", "centimetres");
        Add(Units.Length.Meter, "meter", "meters", "metre", "metres");
        Add(Units.Length.Kilometer, "kilometer", "kilometers", "kilometre", "kilometres");

        // Time
        Add(Units.Time.Millisecond, "millisecond", "milliseconds");
        Add(Units.Time.Second, "sec", "secs", "second", "seconds");
        Add(Units.Time.Minute, "mins", "minute", "minutes");
        Add(Units.Time.Hour, "hr", "hrs", "hour", "hours");
        Add(Units.Time.Day, "days");
        Add(Units.Time.Week, "weeks");

        return map;
    }
}

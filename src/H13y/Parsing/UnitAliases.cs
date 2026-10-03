namespace H13y;

/// <summary>
/// Resolves unit symbols and their aliases to the registered <see cref="Unit"/> instances.
/// </summary>
/// <remarks>
/// Users type units in many different ways: "kg", "KG", "kilogram", "kilos". The lookup is
/// case-insensitive and both symbols and full English names are registered.
/// </remarks>
internal static class UnitAliases
{
    private static readonly Dictionary<string, Unit> Map = BuildMap();

    /// <summary>Resolves a symbol or alias to its <see cref="Unit"/>, or returns <c>null</c> if unknown.</summary>
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

        // Volume
        Add(Units.Volume.Milliliter, "milliliter", "milliliters", "millilitre", "millilitres");
        Add(Units.Volume.Centiliter, "centiliter", "centiliters", "centilitre", "centilitres");
        Add(Units.Volume.Liter, "liter", "liters", "litre", "litres");
        Add(Units.Volume.CubicMeter, "cubicmeter", "cubicmeters", "cubicmetre", "cubicmetres");

        // Area
        Add(Units.Area.SquareMillimeter, "squaremillimeter", "squaremillimeters");
        Add(Units.Area.SquareCentimeter, "squarecentimeter", "squarecentimeters");
        Add(Units.Area.SquareMeter, "squaremeter", "squaremeters", "squaremetre", "squaremetres");
        Add(Units.Area.Hectare, "hectares");
        Add(Units.Area.SquareKilometer, "squarekilometer", "squarekilometers");

        // Temperature
        Add(Units.Temperature.Celsius, "c", "celsius", "centigrade", "°c");
        Add(Units.Temperature.Fahrenheit, "f", "fahrenheit", "°f");
        Add(Units.Temperature.Kelvin, "k", "kelvin");

        return map;
    }
}

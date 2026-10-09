namespace H13y;

/// <summary>
/// Resolves unit symbols and their aliases to the registered <see cref="Unit"/> instances.
/// </summary>
internal static class UnitAliases
{
    private static readonly Dictionary<string, Unit> SymbolMap;
    private static readonly Dictionary<string, Unit> AliasMap;

    static UnitAliases()
    {
        (SymbolMap, AliasMap) = BuildMaps();
    }

    public static Unit? Resolve(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return null;

        var key = symbol.Trim();

        if (SymbolMap.TryGetValue(key, out var exactUnit))
            return exactUnit;

        return AliasMap.TryGetValue(key, out var aliasUnit) ? aliasUnit : null;
    }

    private static (Dictionary<string, Unit> Symbols, Dictionary<string, Unit> Aliases) BuildMaps()
    {
        var symbols = new Dictionary<string, Unit>(StringComparer.Ordinal);
        var aliasesMap = new Dictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);

        void Add(Unit unit, params string[] aliases)
        {
            symbols[unit.Symbol] = unit;

            if (unit.IecSymbol is not null)
            {
                symbols[unit.IecSymbol] = unit;
                aliasesMap[unit.IecSymbol] = unit;
            }

            foreach (var alias in aliases)
                aliasesMap[alias] = unit;
        }

        // Data
        Add(Units.Data.Byte, "byte", "bytes");
        Add(Units.Data.Kilobyte, "kilobyte", "kilobytes", "kibibyte", "kibibytes");
        Add(Units.Data.Megabyte, "megabyte", "megabytes", "mebibyte", "mebibytes");
        Add(Units.Data.Gigabyte, "gigabyte", "gigabytes", "gibibyte", "gibibytes");
        Add(Units.Data.Terabyte, "terabyte", "terabytes", "tebibyte", "tebibytes");
        Add(Units.Data.Petabyte, "petabyte", "petabytes", "pebibyte", "pebibytes");

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

        // Speed
        Add(Units.Speed.MeterPerSecond, "mps", "meterspersecond");
        Add(Units.Speed.KilometerPerHour, "kmh", "kph", "kilometersperhour", "kilometresperhour");
        Add(Units.Speed.MilePerHour, "milesperhour");
        Add(Units.Speed.Knot, "knots", "kt");
        Add(Units.Speed.FootPerSecond, "fps", "feetpersecond");

        // Energy
        Add(Units.Energy.Joule, "joule", "joules");
        Add(Units.Energy.Kilojoule, "kilojoule", "kilojoules");
        Add(Units.Energy.Megajoule, "megajoule", "megajoules");
        Add(Units.Energy.Calorie, "calorie", "calories");
        Add(Units.Energy.Kilocalorie, "kilocalorie", "kilocalories", "foodcalorie");
        Add(Units.Energy.WattHour, "watthour", "watthours");
        Add(Units.Energy.KilowattHour, "kilowatthour", "kilowatthours", "kwhour");

        // Power
        Add(Units.Power.Watt, "watt", "watts");
        Add(Units.Power.Kilowatt, "kilowatt", "kilowatts");
        Add(Units.Power.Megawatt, "megawatt", "megawatts");
        Add(Units.Power.Gigawatt, "gigawatt", "gigawatts");
        Add(Units.Power.Horsepower, "horsepower");

        // Pressure
        Add(Units.Pressure.Pascal, "pascal", "pascals");
        Add(Units.Pressure.Kilopascal, "kilopascal", "kilopascals");
        Add(Units.Pressure.Megapascal, "megapascal", "megapascals");
        Add(Units.Pressure.Bar, "bars");
        Add(Units.Pressure.Psi, "poundspersquareinch");
        Add(Units.Pressure.Atmosphere, "atmospheres");

        // Frequency
        Add(Units.Frequency.Hertz, "hertz");
        Add(Units.Frequency.Kilohertz, "kilohertz");
        Add(Units.Frequency.Megahertz, "megahertz");
        Add(Units.Frequency.Gigahertz, "gigahertz");

        // Angle
        Add(Units.Angle.Radian, "radians");
        Add(Units.Angle.Degree, "degree", "degrees", "°");
        Add(Units.Angle.Gradian, "gradians");
        Add(Units.Angle.Turn, "turns", "revolution", "revolutions");

        return (symbols, aliasesMap);
    }
}

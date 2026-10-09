namespace H13y;

/// <summary>
/// Maps compact unit symbols to their full English names and plural forms.
/// Used when <see cref="HumanizeOptions.UnitStyle"/> is <see cref="UnitStyle.FullName"/>.
/// </summary>
/// <remarks>
/// The table covers every symbol the library produces, plus "mo" and "y" which are
/// used only by <see cref="H.RelativeTime"/>. Unknown symbols fall through unchanged,
/// so future units degrade gracefully to their compact form.
/// </remarks>
internal static class UnitNames
{
    private static readonly Dictionary<string, (string Singular, string Plural)> Map =
        new(StringComparer.Ordinal)
        {
            // Data
            ["B"] = ("byte", "bytes"),
            ["KB"] = ("kilobyte", "kilobytes"),
            ["MB"] = ("megabyte", "megabytes"),
            ["GB"] = ("gigabyte", "gigabytes"),
            ["TB"] = ("terabyte", "terabytes"),
            ["PB"] = ("petabyte", "petabytes"),
            ["KiB"] = ("kibibyte", "kibibytes"),
            ["MiB"] = ("mebibyte", "mebibytes"),
            ["GiB"] = ("gibibyte", "gibibytes"),
            ["TiB"] = ("tebibyte", "tebibytes"),
            ["PiB"] = ("pebibyte", "pebibytes"),

            // Mass
            ["mg"] = ("milligram", "milligrams"),
            ["g"] = ("gram", "grams"),
            ["kg"] = ("kilogram", "kilograms"),
            ["t"] = ("tonne", "tonnes"),

            // Length
            ["mm"] = ("millimeter", "millimeters"),
            ["cm"] = ("centimeter", "centimeters"),
            ["m"] = ("meter", "meters"),
            ["km"] = ("kilometer", "kilometers"),

            // Time
            ["ms"] = ("millisecond", "milliseconds"),
            ["s"] = ("second", "seconds"),
            ["min"] = ("minute", "minutes"),
            ["h"] = ("hour", "hours"),
            ["d"] = ("day", "days"),
            ["w"] = ("week", "weeks"),

            // Relative-time only (not a registered Unit).
            ["mo"] = ("month", "months"),
            ["y"] = ("year", "years"),

            // Volume
            ["ml"] = ("milliliter", "milliliters"),
            ["cl"] = ("centiliter", "centiliters"),
            ["l"] = ("liter", "liters"),
            ["m3"] = ("cubic meter", "cubic meters"),

            // Area
            ["mm2"] = ("square millimeter", "square millimeters"),
            ["cm2"] = ("square centimeter", "square centimeters"),
            ["m2"] = ("square meter", "square meters"),
            ["ha"] = ("hectare", "hectares"),
            ["km2"] = ("square kilometer", "square kilometers"),

            // Temperature
            ["°C"] = ("degree Celsius", "degrees Celsius"),
            ["°F"] = ("degree Fahrenheit", "degrees Fahrenheit"),
            ["K"] = ("kelvin", "kelvin"),

            // Speed
            ["m/s"] = ("meter per second", "meters per second"),
            ["km/h"] = ("kilometer per hour", "kilometers per hour"),
            ["mph"] = ("mile per hour", "miles per hour"),
            ["kn"] = ("knot", "knots"),
            ["ft/s"] = ("foot per second", "feet per second"),

            // Energy
            ["J"] = ("joule", "joules"),
            ["kJ"] = ("kilojoule", "kilojoules"),
            ["MJ"] = ("megajoule", "megajoules"),
            ["cal"] = ("calorie", "calories"),
            ["kcal"] = ("kilocalorie", "kilocalories"),
            ["Wh"] = ("watt-hour", "watt-hours"),
            ["kWh"] = ("kilowatt-hour", "kilowatt-hours"),

            // Power
            ["W"] = ("watt", "watts"),
            ["kW"] = ("kilowatt", "kilowatts"),
            ["MW"] = ("megawatt", "megawatts"),
            ["GW"] = ("gigawatt", "gigawatts"),
            ["hp"] = ("horsepower", "horsepower"),

            // Pressure
            ["Pa"] = ("pascal", "pascals"),
            ["kPa"] = ("kilopascal", "kilopascals"),
            ["MPa"] = ("megapascal", "megapascals"),
            ["bar"] = ("bar", "bar"),
            ["psi"] = ("pound per square inch", "pounds per square inch"),
            ["atm"] = ("atmosphere", "atmospheres"),

            // Frequency
            ["Hz"] = ("hertz", "hertz"),
            ["kHz"] = ("kilohertz", "kilohertz"),
            ["MHz"] = ("megahertz", "megahertz"),
            ["GHz"] = ("gigahertz", "gigahertz"),

            // Angle
            ["rad"] = ("radian", "radians"),
            ["deg"] = ("degree", "degrees"),
            ["grad"] = ("gradian", "gradians"),
            ["turn"] = ("turn", "turns"),
        };

    /// <summary>
    /// Resolves the label to display for <paramref name="symbol"/> given the value and options.
    /// Returns the symbol unchanged when the style is <see cref="UnitStyle.Symbol"/> or the
    /// symbol is not in the table.
    /// </summary>
    public static string Resolve(string symbol, double value, UnitStyle style, bool pluralize)
    {
        if (style == UnitStyle.Symbol)
            return symbol;

        if (!Map.TryGetValue(symbol, out var names))
            return symbol;

        if (!pluralize)
            return names.Singular;

        var isSingular = Math.Abs(Math.Abs(value) - 1.0) < 1e-9;
        return isSingular ? names.Singular : names.Plural;
    }
}

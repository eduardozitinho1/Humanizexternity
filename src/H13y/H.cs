using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization and conversion calls.
/// </summary>
public static class H
{
    /// <summary>Humanizes a byte count into a string such as "1 GB" or "1.5 KB".</summary>
    public static string Bytes(long bytes, HumanizeOptions? options = null)
        => DataSize.FromBytes(bytes).Humanize(options);

    /// <summary>Humanizes a mass in grams into a string such as "1.5 kg" or "500 g".</summary>
    public static string Grams(double grams, HumanizeOptions? options = null)
        => Mass.FromGrams(grams).Humanize(options);

    /// <summary>Humanizes a length in meters into a string such as "1.5 km" or "5 mm".</summary>
    public static string Meters(double meters, HumanizeOptions? options = null)
        => Length.FromMeters(meters).Humanize(options);

    /// <summary>Humanizes a duration in seconds into a compound string such as "1 h 30 min 5 s".</summary>
    public static string Seconds(double seconds, HumanizeOptions? options = null)
        => Duration.FromSeconds(seconds).Humanize(options);

    /// <summary>Humanizes a volume in liters into a string such as "1.5 l" or "500 ml".</summary>
    public static string Liters(double liters, HumanizeOptions? options = null)
        => Volume.FromLiters(liters).Humanize(options);

    /// <summary>Humanizes an area in square meters into a string such as "1.5 km2" or "500 m2".</summary>
    public static string SquareMeters(double squareMeters, HumanizeOptions? options = null)
        => Area.FromSquareMeters(squareMeters).Humanize(options);

    /// <summary>Humanizes a temperature given in degrees Celsius.</summary>
    public static string Celsius(double celsius, HumanizeOptions? options = null)
        => Temperature.FromCelsius(celsius).Humanize(TemperatureScale.Celsius, options);

    /// <summary>Humanizes a temperature given in degrees Fahrenheit.</summary>
    public static string Fahrenheit(double fahrenheit, HumanizeOptions? options = null)
        => Temperature.FromFahrenheit(fahrenheit).Humanize(TemperatureScale.Fahrenheit, options);

    /// <summary>Humanizes a temperature given in kelvin.</summary>
    public static string Kelvin(double kelvin, HumanizeOptions? options = null)
        => Temperature.FromKelvin(kelvin).Humanize(TemperatureScale.Kelvin, options);

    /// <summary>
    /// Converts a numeric value from one unit to another within the same dimension.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="from">The source unit.</param>
    /// <param name="to">The target unit.</param>
    /// <returns>The converted numeric value.</returns>
    /// <exception cref="ArgumentException">Thrown when the units belong to different dimensions.</exception>
    public static double Convert(double value, Unit from, Unit to)
        => UnitConverter.Convert(value, from, to);

    /// <summary>
    /// Parses a human-readable string and converts the result to the target unit.
    /// </summary>
    /// <param name="text">Input such as "1.5 GB".</param>
    /// <param name="to">The target unit.</param>
    /// <returns>The value expressed in <paramref name="to"/>.</returns>
    /// <exception cref="FormatException">Thrown when the input cannot be parsed.</exception>
    /// <exception cref="ArgumentException">Thrown when the parsed unit and target unit belong to different dimensions.</exception>
    public static double Convert(string text, Unit to)
    {
        var measure = UnitParser.Parse(text);
        return UnitConverter.Convert(measure.Value, measure.Unit, to);
    }

    /// <summary>Picks the best unit for the given base value and dimension, then formats it.</summary>
    public static string Best(double baseValue, Dimension dimension, HumanizeOptions? options = null)
        => HumanizeFormatter.Format(baseValue, dimension, options);

    /// <summary>Parses a human-readable string such as "1.5 kg" into a <see cref="Measure"/>.</summary>
    public static Measure Parse(string text) => UnitParser.Parse(text);
}

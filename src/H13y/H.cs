using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization, conversion, and parsing calls.
/// </summary>
public static class H
{
    public static string Bytes(long bytes, HumanizeOptions? options = null)
        => DataSize.FromBytes(bytes).Humanize(options);

    public static string Grams(double grams, HumanizeOptions? options = null)
        => Mass.FromGrams(grams).Humanize(options);

    public static string Meters(double meters, HumanizeOptions? options = null)
        => Length.FromMeters(meters).Humanize(options);

    public static string Seconds(double seconds, HumanizeOptions? options = null)
        => Duration.FromSeconds(seconds).Humanize(options);

    public static string TimeSpan(System.TimeSpan span, HumanizeOptions? options = null)
        => Duration.FromTimeSpan(span).Humanize(options);

    public static string Liters(double liters, HumanizeOptions? options = null)
        => Volume.FromLiters(liters).Humanize(options);

    public static string SquareMeters(double squareMeters, HumanizeOptions? options = null)
        => Area.FromSquareMeters(squareMeters).Humanize(options);

    public static string Celsius(double celsius, HumanizeOptions? options = null)
        => Temperature.FromCelsius(celsius).Humanize(TemperatureScale.Celsius, options);

    public static string Fahrenheit(double fahrenheit, HumanizeOptions? options = null)
        => Temperature.FromFahrenheit(fahrenheit).Humanize(TemperatureScale.Fahrenheit, options);

    public static string Kelvin(double kelvin, HumanizeOptions? options = null)
        => Temperature.FromKelvin(kelvin).Humanize(TemperatureScale.Kelvin, options);

    public static double Convert(double value, Unit from, Unit to)
        => UnitConverter.Convert(value, from, to);

    public static double Convert(string text, Unit to)
    {
        var measure = UnitParser.Parse(text);
        return UnitConverter.Convert(measure.Value, measure.Unit, to);
    }

    public static System.TimeSpan ParseTimeSpan(string text)
    {
        var measure = UnitParser.Parse(text);
        if (measure.Unit.Dimension != Dimension.Time)
            throw new FormatException($"Input '{text}' is not a duration.");
        return System.TimeSpan.FromSeconds(measure.ToBase());
    }

    public static bool TryParseTimeSpan(string text, out System.TimeSpan span)
    {
        try
        {
            span = ParseTimeSpan(text);
            return true;
        }
        catch (FormatException)
        {
            span = default;
            return false;
        }
    }

    public static string Best(double baseValue, Dimension dimension, HumanizeOptions? options = null)
        => HumanizeFormatter.Format(baseValue, dimension, options);

    public static Measure Parse(string text) => UnitParser.Parse(text);

    /// <summary>
    /// Attempts to parse a human-readable string into a <see cref="DataSize"/>.
    /// Fails if the input belongs to another dimension.
    /// </summary>
    public static bool TryParseDataSize(string text, out DataSize size)
    {
        if (TryParseDimension(text, Dimension.Data, out var value))
        {
            size = DataSize.FromBytes((long)value);
            return true;
        }
        size = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Mass"/>.</summary>
    public static bool TryParseMass(string text, out Mass mass)
    {
        if (TryParseDimension(text, Dimension.Mass, out var value))
        {
            mass = Mass.FromGrams(value);
            return true;
        }
        mass = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Length"/>.</summary>
    public static bool TryParseLength(string text, out Length length)
    {
        if (TryParseDimension(text, Dimension.Length, out var value))
        {
            length = Length.FromMeters(value);
            return true;
        }
        length = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Duration"/>.</summary>
    public static bool TryParseDuration(string text, out Duration duration)
    {
        if (TryParseDimension(text, Dimension.Time, out var value))
        {
            duration = Duration.FromSeconds(value);
            return true;
        }
        duration = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Volume"/>.</summary>
    public static bool TryParseVolume(string text, out Volume volume)
    {
        if (TryParseDimension(text, Dimension.Volume, out var value))
        {
            volume = Volume.FromLiters(value);
            return true;
        }
        volume = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into an <see cref="Area"/>.</summary>
    public static bool TryParseArea(string text, out Area area)
    {
        if (TryParseDimension(text, Dimension.Area, out var value))
        {
            area = Area.FromSquareMeters(value);
            return true;
        }
        area = default;
        return false;
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Temperature"/>.</summary>
    public static bool TryParseTemperature(string text, out Temperature temperature)
    {
        if (TryParseDimension(text, Dimension.Temperature, out var value))
        {
            temperature = Temperature.FromKelvin(value);
            return true;
        }
        temperature = default;
        return false;
    }

    private static bool TryParseDimension(string text, Dimension dimension, out double baseValue)
    {
        baseValue = 0;
        try
        {
            var measure = UnitParser.Parse(text);
            if (measure.Unit.Dimension != dimension)
                return false;
            baseValue = measure.ToBase();
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

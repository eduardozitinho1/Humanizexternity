using System.Globalization;
using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization, conversion, and parsing calls.
/// </summary>
public static class H
{
    public static string Bytes(long bytes, HumanizeOptions? options = null) =>
        DataSize.FromBytes(bytes).Humanize(options);

    public static string Grams(double grams, HumanizeOptions? options = null) =>
        Mass.FromGrams(grams).Humanize(options);

    public static string Meters(double meters, HumanizeOptions? options = null) =>
        Length.FromMeters(meters).Humanize(options);

    public static string Seconds(double seconds, HumanizeOptions? options = null) =>
        Duration.FromSeconds(seconds).Humanize(options);

    public static string TimeSpan(System.TimeSpan span, HumanizeOptions? options = null) =>
        Duration.FromTimeSpan(span).Humanize(options);

    public static string Liters(double liters, HumanizeOptions? options = null) =>
        Volume.FromLiters(liters).Humanize(options);

    public static string SquareMeters(double squareMeters, HumanizeOptions? options = null) =>
        Area.FromSquareMeters(squareMeters).Humanize(options);

    public static string Celsius(double celsius, HumanizeOptions? options = null) =>
        Temperature.FromCelsius(celsius).Humanize(TemperatureScale.Celsius, options);

    public static string Fahrenheit(double fahrenheit, HumanizeOptions? options = null) =>
        Temperature.FromFahrenheit(fahrenheit).Humanize(TemperatureScale.Fahrenheit, options);

    public static string Kelvin(double kelvin, HumanizeOptions? options = null) =>
        Temperature.FromKelvin(kelvin).Humanize(TemperatureScale.Kelvin, options);

    public static string MetersPerSecond(double mps, HumanizeOptions? options = null) =>
        Speed.FromMetersPerSecond(mps).Humanize(options);

    public static string KilometersPerHour(double kph, HumanizeOptions? options = null) =>
        Speed.FromKilometersPerHour(kph).Humanize(options);

    public static string MilesPerHour(double mph, HumanizeOptions? options = null) =>
        Speed.FromMilesPerHour(mph).Humanize(options);

    public static string Joules(double joules, HumanizeOptions? options = null) =>
        Energy.FromJoules(joules).Humanize(options);

    public static string Kilojoules(double kj, HumanizeOptions? options = null) =>
        Energy.FromKilojoules(kj).Humanize(options);

    public static string KilowattHours(double kwh, HumanizeOptions? options = null) =>
        Energy.FromKilowattHours(kwh).Humanize(options);

    public static string Kilocalories(double kcal, HumanizeOptions? options = null) =>
        Energy.FromKilocalories(kcal).Humanize(options);

    public static string Watts(double watts, HumanizeOptions? options = null) =>
        Power.FromWatts(watts).Humanize(options);

    public static string Kilowatts(double kw, HumanizeOptions? options = null) =>
        Power.FromKilowatts(kw).Humanize(options);

    public static string Megawatts(double mw, HumanizeOptions? options = null) =>
        Power.FromMegawatts(mw).Humanize(options);

    public static string Horsepower(double hp, HumanizeOptions? options = null) =>
        Power.FromHorsepower(hp).Humanize(options);

    public static string Pascals(double pa, HumanizeOptions? options = null) =>
        Pressure.FromPascals(pa).Humanize(options);

    public static string Kilopascals(double kpa, HumanizeOptions? options = null) =>
        Pressure.FromKilopascals(kpa).Humanize(options);

    public static string Bars(double bar, HumanizeOptions? options = null) =>
        Pressure.FromBars(bar).Humanize(options);

    public static string Psi(double psi, HumanizeOptions? options = null) =>
        Pressure.FromPsi(psi).Humanize(options);

    public static string Hertz(double hz, HumanizeOptions? options = null) =>
        Frequency.FromHertz(hz).Humanize(options);

    public static string Kilohertz(double khz, HumanizeOptions? options = null) =>
        Frequency.FromKilohertz(khz).Humanize(options);

    public static string Megahertz(double mhz, HumanizeOptions? options = null) =>
        Frequency.FromMegahertz(mhz).Humanize(options);

    public static string Gigahertz(double ghz, HumanizeOptions? options = null) =>
        Frequency.FromGigahertz(ghz).Humanize(options);

    public static string Radians(double rad, HumanizeOptions? options = null) =>
        Angle.FromRadians(rad).Humanize(options);

    public static string Degrees(double deg, HumanizeOptions? options = null) =>
        Angle.FromDegrees(deg).Humanize(options);

    /// <summary>
    /// Converts a numeric value from one unit to another within the same dimension.
    /// </summary>
    public static double Convert(double value, Unit from, Unit to) =>
        UnitConverter.Convert(value, from, to);

    /// <summary>
    /// Parses a human-readable string using the given culture and converts the result to the target unit.
    /// </summary>
    /// <param name="text">The input text, such as "1.5 GB".</param>
    /// <param name="to">The target unit.</param>
    /// <param name="culture">The culture used to interpret the decimal separator. Must not be null.</param>
    public static double Convert(string text, Unit to, CultureInfo culture)
    {
        var measure = UnitParser.Parse(text, culture);
        return UnitConverter.Convert(measure.Value, measure.Unit, to);
    }

    /// <summary>
    /// Parses a human-readable duration into a <see cref="System.TimeSpan"/> using the given culture.
    /// </summary>
    public static System.TimeSpan ParseTimeSpan(string text, CultureInfo culture)
    {
        var measure = UnitParser.Parse(text, culture);
        if (measure.Unit.Dimension != Dimension.Time)
            throw new FormatException($"Input '{text}' is not a duration.");
        return System.TimeSpan.FromSeconds(measure.ToBase());
    }

    /// <summary>Attempts to parse a duration into a <see cref="System.TimeSpan"/> without throwing.</summary>
    public static bool TryParseTimeSpan(string text, CultureInfo culture, out System.TimeSpan span)
    {
        try
        {
            span = ParseTimeSpan(text, culture);
            return true;
        }
        catch (FormatException)
        {
            span = default;
            return false;
        }
    }

    /// <summary>Picks the best unit for the given base value and dimension, then formats it.</summary>
    public static string Best(
        double baseValue,
        Dimension dimension,
        HumanizeOptions? options = null
    ) => HumanizeFormatter.Format(baseValue, dimension, options);

    /// <summary>
    /// Parses a human-readable string using the given culture.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="culture"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the input cannot be parsed.</exception>
    public static Measure Parse(string text, CultureInfo culture) =>
        UnitParser.Parse(text, culture);

    public static bool TryParseDataSize(string text, CultureInfo culture, out DataSize size)
    {
        if (TryParseDimension(text, culture, Dimension.Data, out var value))
        {
            size = DataSize.FromBytes((long)value);
            return true;
        }
        size = default;
        return false;
    }

    public static bool TryParseMass(string text, CultureInfo culture, out Mass mass)
    {
        if (TryParseDimension(text, culture, Dimension.Mass, out var value))
        {
            mass = Mass.FromGrams(value);
            return true;
        }
        mass = default;
        return false;
    }

    public static bool TryParseLength(string text, CultureInfo culture, out Length length)
    {
        if (TryParseDimension(text, culture, Dimension.Length, out var value))
        {
            length = Length.FromMeters(value);
            return true;
        }
        length = default;
        return false;
    }

    public static bool TryParseDuration(string text, CultureInfo culture, out Duration duration)
    {
        if (TryParseDimension(text, culture, Dimension.Time, out var value))
        {
            duration = Duration.FromSeconds(value);
            return true;
        }
        duration = default;
        return false;
    }

    public static bool TryParseVolume(string text, CultureInfo culture, out Volume volume)
    {
        if (TryParseDimension(text, culture, Dimension.Volume, out var value))
        {
            volume = Volume.FromLiters(value);
            return true;
        }
        volume = default;
        return false;
    }

    public static bool TryParseArea(string text, CultureInfo culture, out Area area)
    {
        if (TryParseDimension(text, culture, Dimension.Area, out var value))
        {
            area = Area.FromSquareMeters(value);
            return true;
        }
        area = default;
        return false;
    }

    public static bool TryParseTemperature(
        string text,
        CultureInfo culture,
        out Temperature temperature
    )
    {
        if (TryParseDimension(text, culture, Dimension.Temperature, out var value))
        {
            temperature = Temperature.FromKelvin(value);
            return true;
        }
        temperature = default;
        return false;
    }

    private static bool TryParseDimension(
        string text,
        CultureInfo culture,
        Dimension dimension,
        out double baseValue
    )
    {
        baseValue = 0;
        try
        {
            var measure = UnitParser.Parse(text, culture);
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

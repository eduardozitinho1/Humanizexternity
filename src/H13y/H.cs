using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization calls.
/// </summary>
/// <remarks>
/// This is the entry point most users reach for. It exposes one method per dimension plus
/// generic helpers for auto-selecting a unit and parsing strings.
/// </remarks>
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

    /// <summary>Picks the best unit for the given base value and dimension, then formats it.</summary>
    public static string Best(double baseValue, Dimension dimension, HumanizeOptions? options = null)
        => HumanizeFormatter.Format(baseValue, dimension, options);

    /// <summary>Parses a human-readable string such as "1.5 kg" into a <see cref="Measure"/>.</summary>
    public static Measure Parse(string text) => UnitParser.Parse(text);
}

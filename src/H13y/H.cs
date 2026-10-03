using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization calls.
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

    /// <summary>Picks the best unit for the given base value and dimension.</summary>
    public static string Best(double baseValue, Dimension dimension, HumanizeOptions? options = null)
        => HumanizeFormatter.Format(baseValue, dimension, options);

    /// <summary>Parses a human-readable string into a <see cref="Measure"/>.</summary>
    public static Measure Parse(string text) => UnitParser.Parse(text);
}

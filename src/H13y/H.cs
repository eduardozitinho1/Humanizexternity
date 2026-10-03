using H13y.Measures;

namespace H13y;

/// <summary>
/// Short facade over the most common humanization calls.
/// </summary>
/// <remarks>
/// This is the entry point most users reach for. It exposes one method per dimension plus
/// generic helpers for auto-selecting a unit and parsing strings.
///
/// Every method here is a thin wrapper over a typed measure in <c>H13y.Measures</c>. When
/// you need access to conversions, arithmetic, or the underlying values, use those typed
/// records directly. <see cref="H"/> is optimized for the common case: turn a number into
/// a display string.
///
/// The class is static and the type name is a single letter to keep call sites short and
/// readable. Combined with the namespace <c>H13y</c>, usage looks like this:
///
/// <code>
/// using H13y;
/// Console.WriteLine(H.Bytes(1073741824)); // "1 GB"
/// </code>
/// </remarks>
public static class H
{
    /// <summary>
    /// Humanizes a byte count into a string such as "1 GB" or "1.5 KB".
    /// </summary>
    /// <param name="bytes">The number of bytes.</param>
    /// <param name="options">Optional formatting options.</param>
    public static string Bytes(long bytes, HumanizeOptions? options = null)
        => DataSize.FromBytes(bytes).Humanize(options);

    /// <summary>
    /// Humanizes a mass in grams into a string such as "1.5 kg" or "500 g".
    /// </summary>
    /// <param name="grams">The mass in grams.</param>
    /// <param name="options">Optional formatting options.</param>
    public static string Grams(double grams, HumanizeOptions? options = null)
        => Mass.FromGrams(grams).Humanize(options);

    /// <summary>
    /// Humanizes a length in meters into a string such as "1.5 km" or "5 mm".
    /// </summary>
    /// <param name="meters">The length in meters.</param>
    /// <param name="options">Optional formatting options.</param>
    public static string Meters(double meters, HumanizeOptions? options = null)
        => Length.FromMeters(meters).Humanize(options);

    /// <summary>
    /// Humanizes a duration in seconds into a compound string such as "1 h 30 min 5 s".
    /// </summary>
    /// <param name="seconds">The duration in seconds.</param>
    /// <param name="options">Optional formatting options.</param>
    public static string Seconds(double seconds, HumanizeOptions? options = null)
        => Duration.FromSeconds(seconds).Humanize(options);

    /// <summary>
    /// Picks the best unit for the given base value and dimension, then formats it.
    /// </summary>
    /// <param name="baseValue">The value expressed in the dimension's base unit.</param>
    /// <param name="dimension">The dimension to use when selecting the unit.</param>
    /// <param name="options">Optional formatting options.</param>
    /// <returns>A human-readable string such as "1.5 kg".</returns>
    /// <remarks>
    /// Use this method when you have a value in the base unit but do not know which typed
    /// record to use — for example, when iterating over mixed data loaded from configuration.
    /// For a single known dimension, prefer the dedicated method such as <see cref="Grams"/>.
    /// </remarks>
    public static string Best(double baseValue, Dimension dimension, HumanizeOptions? options = null)
        => HumanizeFormatter.Format(baseValue, dimension, options);

    /// <summary>
    /// Parses a human-readable string such as "1.5 kg" into a <see cref="Measure"/>.
    /// </summary>
    /// <param name="text">The input string to parse.</param>
    /// <returns>A <see cref="Measure"/> with the parsed value and unit.</returns>
    /// <exception cref="FormatException">Thrown when the input is empty, malformed, or uses an unknown unit.</exception>
    public static Measure Parse(string text) => UnitParser.Parse(text);
}

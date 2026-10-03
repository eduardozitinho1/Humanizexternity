namespace H13y.Measures;

/// <summary>
/// A span of time, stored internally as a number of seconds.
/// </summary>
/// <remarks>
/// <see cref="Duration"/> represents intervals, elapsed time, and timeouts. The base unit
/// is the second, which is the SI standard and matches the convention used by Unix
/// timestamps and most APIs.
///
/// Unlike <see cref="DataSize"/>, <see cref="Mass"/>, and <see cref="Length"/>, time does
/// not scale by powers of ten. This means humanization uses a compound format such as
/// "1 h 30 min 5 s" rather than picking a single best unit. See
/// <see cref="HumanizeFormatter.FormatDuration"/> for the algorithm.
/// </remarks>
/// <param name="Seconds">The duration in seconds.</param>
public readonly record struct Duration(double Seconds)
{
    /// <summary>Creates a <see cref="Duration"/> from a number of milliseconds.</summary>
    public static Duration FromMilliseconds(double ms) => new(ms / 1000);

    /// <summary>Creates a <see cref="Duration"/> from a number of seconds.</summary>
    public static Duration FromSeconds(double s) => new(s);

    /// <summary>Creates a <see cref="Duration"/> from a number of minutes.</summary>
    public static Duration FromMinutes(double m) => new(m * 60);

    /// <summary>Creates a <see cref="Duration"/> from a number of hours.</summary>
    public static Duration FromHours(double h) => new(h * 3600);

    /// <summary>Creates a <see cref="Duration"/> from a number of days.</summary>
    public static Duration FromDays(double d) => new(d * 86400);

    /// <summary>Creates a <see cref="Duration"/> from a number of weeks.</summary>
    public static Duration FromWeeks(double w) => new(w * 604800);

    /// <summary>Returns the duration expressed in milliseconds.</summary>
    public double ToMilliseconds() => Seconds * 1000;

    /// <summary>Returns the duration expressed in minutes.</summary>
    public double ToMinutes() => Seconds / 60;

    /// <summary>Returns the duration expressed in hours.</summary>
    public double ToHours() => Seconds / 3600;

    /// <summary>Returns the duration expressed in days.</summary>
    public double ToDays() => Seconds / 86400;

    /// <summary>Returns the duration expressed in weeks.</summary>
    public double ToWeeks() => Seconds / 604800;

    /// <summary>
    /// Renders the duration as a human-readable string such as "1 h 30 min" or "45 s".
    /// </summary>
    /// <param name="options">Optional formatting options. When omitted, <see cref="HumanizeOptions.Default"/> is used.</param>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.FormatDuration(Seconds, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

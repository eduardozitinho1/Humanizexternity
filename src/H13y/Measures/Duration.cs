namespace H13y.Measures;

/// <summary>
/// A span of time, stored internally as a number of seconds.
/// </summary>
/// <param name="Seconds">The duration in seconds.</param>
public readonly record struct Duration(double Seconds) : IComparable<Duration>
{
    public static Duration FromMilliseconds(double ms) => new(ms / 1000);
    public static Duration FromSeconds(double s) => new(s);
    public static Duration FromMinutes(double m) => new(m * 60);
    public static Duration FromHours(double h) => new(h * 3600);
    public static Duration FromDays(double d) => new(d * 86400);
    public static Duration FromWeeks(double w) => new(w * 604800);

    public double ToMilliseconds() => Seconds * 1000;
    public double ToMinutes() => Seconds / 60;
    public double ToHours() => Seconds / 3600;
    public double ToDays() => Seconds / 86400;
    public double ToWeeks() => Seconds / 604800;

    public static Duration operator +(Duration left, Duration right) => new(left.Seconds + right.Seconds);
    public static Duration operator -(Duration left, Duration right) => new(left.Seconds - right.Seconds);
    public static bool operator <(Duration left, Duration right) => left.Seconds < right.Seconds;
    public static bool operator <=(Duration left, Duration right) => left.Seconds <= right.Seconds;
    public static bool operator >(Duration left, Duration right) => left.Seconds > right.Seconds;
    public static bool operator >=(Duration left, Duration right) => left.Seconds >= right.Seconds;

    public int CompareTo(Duration other) => Seconds.CompareTo(other.Seconds);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.FormatDuration(Seconds, options);

    public override string ToString() => Humanize();
}

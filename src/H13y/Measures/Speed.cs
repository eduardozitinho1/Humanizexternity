namespace H13y.Measures;

/// <summary>
/// A speed, stored internally as a number of meters per second.
/// </summary>
/// <param name="MetersPerSecond">The speed in m/s.</param>
public readonly record struct Speed(double MetersPerSecond) : IComparable<Speed>, IFormattable
{
    public static Speed FromMetersPerSecond(double mps) => new(mps);
    public static Speed FromKilometersPerHour(double kph) => new(kph * 1000.0 / 3600);
    public static Speed FromMilesPerHour(double mph) => new(mph * 1609.344 / 3600);
    public static Speed FromKnots(double kn) => new(kn * 1852.0 / 3600);
    public static Speed FromFeetPerSecond(double fps) => new(fps * 0.3048);

    public double ToKilometersPerHour() => MetersPerSecond * 3600.0 / 1000;
    public double ToMilesPerHour() => MetersPerSecond * 3600.0 / 1609.344;
    public double ToKnots() => MetersPerSecond * 3600.0 / 1852;
    public double ToFeetPerSecond() => MetersPerSecond / 0.3048;

    public static Speed operator +(Speed left, Speed right) => new(left.MetersPerSecond + right.MetersPerSecond);
    public static Speed operator -(Speed left, Speed right) => new(left.MetersPerSecond - right.MetersPerSecond);
    public static bool operator <(Speed left, Speed right) => left.MetersPerSecond < right.MetersPerSecond;
    public static bool operator <=(Speed left, Speed right) => left.MetersPerSecond <= right.MetersPerSecond;
    public static bool operator >(Speed left, Speed right) => left.MetersPerSecond > right.MetersPerSecond;
    public static bool operator >=(Speed left, Speed right) => left.MetersPerSecond >= right.MetersPerSecond;

    public int CompareTo(Speed other) => MetersPerSecond.CompareTo(other.MetersPerSecond);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(MetersPerSecond, Dimension.Speed, options);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => FormattableHelpers.FormatScalar(MetersPerSecond, Dimension.Speed, format, formatProvider);

    public override string ToString() => Humanize();
}

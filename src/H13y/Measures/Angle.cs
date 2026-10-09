namespace H13y.Measures;

/// <summary>
/// An angle, stored internally as a number of radians.
/// </summary>
/// <param name="Radians">The angle in radians.</param>
public readonly record struct Angle(double Radians) : IComparable<Angle>, IFormattable
{
    public static Angle FromRadians(double rad) => new(rad);

    public static Angle FromDegrees(double deg) => new(deg * Math.PI / 180);

    public static Angle FromGradians(double grad) => new(grad * Math.PI / 200);

    public static Angle FromTurns(double turn) => new(turn * 2 * Math.PI);

    public double ToDegrees() => Radians * 180 / Math.PI;

    public double ToGradians() => Radians * 200 / Math.PI;

    public double ToTurns() => Radians / (2 * Math.PI);

    public static Angle operator +(Angle left, Angle right) => new(left.Radians + right.Radians);

    public static Angle operator -(Angle left, Angle right) => new(left.Radians - right.Radians);

    public static bool operator <(Angle left, Angle right) => left.Radians < right.Radians;

    public static bool operator <=(Angle left, Angle right) => left.Radians <= right.Radians;

    public static bool operator >(Angle left, Angle right) => left.Radians > right.Radians;

    public static bool operator >=(Angle left, Angle right) => left.Radians >= right.Radians;

    public int CompareTo(Angle other) => Radians.CompareTo(other.Radians);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(Radians, Dimension.Angle, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(Radians, Dimension.Angle, format, formatProvider);

    public override string ToString() => Humanize();
}

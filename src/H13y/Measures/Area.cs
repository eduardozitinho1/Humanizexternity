namespace H13y.Measures;

/// <summary>
/// A quantity of area, stored internally as a number of square meters.
/// </summary>
/// <param name="SquareMeters">The area in square meters.</param>
public readonly record struct Area(double SquareMeters) : IComparable<Area>, IFormattable
{
    public static Area FromSquareMillimeters(double mm2) => new(mm2 / 1_000_000);

    public static Area FromSquareCentimeters(double cm2) => new(cm2 / 10_000);

    public static Area FromSquareMeters(double m2) => new(m2);

    public static Area FromHectares(double ha) => new(ha * 10_000);

    public static Area FromSquareKilometers(double km2) => new(km2 * 1_000_000);

    public double ToSquareMillimeters() => SquareMeters * 1_000_000;

    public double ToSquareCentimeters() => SquareMeters * 10_000;

    public double ToHectares() => SquareMeters / 10_000;

    public double ToSquareKilometers() => SquareMeters / 1_000_000;

    public static Area operator +(Area left, Area right) =>
        new(left.SquareMeters + right.SquareMeters);

    public static Area operator -(Area left, Area right) =>
        new(left.SquareMeters - right.SquareMeters);

    public static bool operator <(Area left, Area right) => left.SquareMeters < right.SquareMeters;

    public static bool operator <=(Area left, Area right) =>
        left.SquareMeters <= right.SquareMeters;

    public static bool operator >(Area left, Area right) => left.SquareMeters > right.SquareMeters;

    public static bool operator >=(Area left, Area right) =>
        left.SquareMeters >= right.SquareMeters;

    public int CompareTo(Area other) => SquareMeters.CompareTo(other.SquareMeters);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(SquareMeters, Dimension.Area, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(SquareMeters, Dimension.Area, format, formatProvider);

    public override string ToString() => Humanize();
}

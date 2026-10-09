namespace H13y.Measures;

/// <summary>
/// A quantity of pressure, stored internally as a number of pascals.
/// </summary>
/// <param name="Pascals">The pressure in pascals.</param>
public readonly record struct Pressure(double Pascals) : IComparable<Pressure>, IFormattable
{
    public static Pressure FromPascals(double pa) => new(pa);

    public static Pressure FromKilopascals(double kpa) => new(kpa * 1000);

    public static Pressure FromMegapascals(double mpa) => new(mpa * 1_000_000);

    public static Pressure FromBars(double bar) => new(bar * 100_000);

    public static Pressure FromPsi(double psi) => new(psi * 6894.757);

    public static Pressure FromAtmospheres(double atm) => new(atm * 101_325);

    public double ToKilopascals() => Pascals / 1000;

    public double ToMegapascals() => Pascals / 1_000_000;

    public double ToBars() => Pascals / 100_000;

    public double ToPsi() => Pascals / 6894.757;

    public double ToAtmospheres() => Pascals / 101_325;

    public static Pressure operator +(Pressure left, Pressure right) =>
        new(left.Pascals + right.Pascals);

    public static Pressure operator -(Pressure left, Pressure right) =>
        new(left.Pascals - right.Pascals);

    public static bool operator <(Pressure left, Pressure right) => left.Pascals < right.Pascals;

    public static bool operator <=(Pressure left, Pressure right) => left.Pascals <= right.Pascals;

    public static bool operator >(Pressure left, Pressure right) => left.Pascals > right.Pascals;

    public static bool operator >=(Pressure left, Pressure right) => left.Pascals >= right.Pascals;

    public int CompareTo(Pressure other) => Pascals.CompareTo(other.Pascals);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(Pascals, Dimension.Pressure, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(Pascals, Dimension.Pressure, format, formatProvider);

    public override string ToString() => Humanize();
}

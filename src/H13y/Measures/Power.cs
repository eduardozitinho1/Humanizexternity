namespace H13y.Measures;

/// <summary>
/// A quantity of power, stored internally as a number of watts.
/// </summary>
/// <param name="Watts">The power in watts.</param>
public readonly record struct Power(double Watts) : IComparable<Power>, IFormattable
{
    public static Power FromWatts(double w) => new(w);
    public static Power FromKilowatts(double kw) => new(kw * 1000);
    public static Power FromMegawatts(double mw) => new(mw * 1_000_000);
    public static Power FromGigawatts(double gw) => new(gw * 1_000_000_000);
    public static Power FromHorsepower(double hp) => new(hp * 745.7);

    public double ToKilowatts() => Watts / 1000;
    public double ToMegawatts() => Watts / 1_000_000;
    public double ToGigawatts() => Watts / 1_000_000_000;
    public double ToHorsepower() => Watts / 745.7;

    public static Power operator +(Power left, Power right) => new(left.Watts + right.Watts);
    public static Power operator -(Power left, Power right) => new(left.Watts - right.Watts);
    public static bool operator <(Power left, Power right) => left.Watts < right.Watts;
    public static bool operator <=(Power left, Power right) => left.Watts <= right.Watts;
    public static bool operator >(Power left, Power right) => left.Watts > right.Watts;
    public static bool operator >=(Power left, Power right) => left.Watts >= right.Watts;

    public int CompareTo(Power other) => Watts.CompareTo(other.Watts);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Watts, Dimension.Power, options);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => FormattableHelpers.FormatScalar(Watts, Dimension.Power, format, formatProvider);

    public override string ToString() => Humanize();
}

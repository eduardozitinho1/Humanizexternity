namespace H13y.Measures;

/// <summary>
/// A frequency, stored internally as a number of hertz.
/// </summary>
/// <param name="Hertz">The frequency in Hz.</param>
public readonly record struct Frequency(double Hertz) : IComparable<Frequency>, IFormattable
{
    public static Frequency FromHertz(double hz) => new(hz);
    public static Frequency FromKilohertz(double khz) => new(khz * 1000);
    public static Frequency FromMegahertz(double mhz) => new(mhz * 1_000_000);
    public static Frequency FromGigahertz(double ghz) => new(ghz * 1_000_000_000);

    public double ToKilohertz() => Hertz / 1000;
    public double ToMegahertz() => Hertz / 1_000_000;
    public double ToGigahertz() => Hertz / 1_000_000_000;

    public static Frequency operator +(Frequency left, Frequency right) => new(left.Hertz + right.Hertz);
    public static Frequency operator -(Frequency left, Frequency right) => new(left.Hertz - right.Hertz);
    public static bool operator <(Frequency left, Frequency right) => left.Hertz < right.Hertz;
    public static bool operator <=(Frequency left, Frequency right) => left.Hertz <= right.Hertz;
    public static bool operator >(Frequency left, Frequency right) => left.Hertz > right.Hertz;
    public static bool operator >=(Frequency left, Frequency right) => left.Hertz >= right.Hertz;

    public int CompareTo(Frequency other) => Hertz.CompareTo(other.Hertz);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Hertz, Dimension.Frequency, options);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => FormattableHelpers.FormatScalar(Hertz, Dimension.Frequency, format, formatProvider);

    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>
/// A data transfer rate, stored internally as bits per second.
/// </summary>
/// <remarks>
/// Distinct from <see cref="Bits"/>, which is an amount of data. A rate is an amount
/// per unit of time. The base unit is the bit per second because that is what network
/// equipment and ISPs use; the byte-per-second units are provided for convenience and
/// are never auto-selected.
/// </remarks>
/// <param name="BitsPerSecond">The rate in bits per second.</param>
public readonly record struct DataRate(double BitsPerSecond) : IComparable<DataRate>, IFormattable
{
    public static DataRate FromBitsPerSecond(double bps) => new(bps);

    public static DataRate FromKilobitsPerSecond(double kbps) => new(kbps * 1_000);

    public static DataRate FromMegabitsPerSecond(double mbps) => new(mbps * 1_000_000);

    public static DataRate FromGigabitsPerSecond(double gbps) => new(gbps * 1_000_000_000);

    public static DataRate FromTerabitsPerSecond(double tbps) => new(tbps * 1_000_000_000_000);

    public static DataRate FromBytesPerSecond(double Bps) => new(Bps * 8);

    public static DataRate FromKilobytesPerSecond(double kBps) => new(kBps * 8_000);

    public static DataRate FromMegabytesPerSecond(double mBps) => new(mBps * 8_000_000);

    public static DataRate FromGigabytesPerSecond(double gBps) => new(gBps * 8_000_000_000);

    public static DataRate FromTerabytesPerSecond(double tBps) => new(tBps * 8_000_000_000_000);

    public double ToKilobitsPerSecond() => BitsPerSecond / 1_000;

    public double ToMegabitsPerSecond() => BitsPerSecond / 1_000_000;

    public double ToGigabitsPerSecond() => BitsPerSecond / 1_000_000_000;

    public double ToTerabitsPerSecond() => BitsPerSecond / 1_000_000_000_000;

    public double ToBytesPerSecond() => BitsPerSecond / 8;

    public double ToKilobytesPerSecond() => BitsPerSecond / 8_000;

    public double ToMegabytesPerSecond() => BitsPerSecond / 8_000_000;

    public double ToGigabytesPerSecond() => BitsPerSecond / 8_000_000_000;

    public double ToTerabytesPerSecond() => BitsPerSecond / 8_000_000_000_000;

    public static DataRate operator +(DataRate left, DataRate right) =>
        new(left.BitsPerSecond + right.BitsPerSecond);

    public static DataRate operator -(DataRate left, DataRate right) =>
        new(left.BitsPerSecond - right.BitsPerSecond);

    public static bool operator <(DataRate left, DataRate right) =>
        left.BitsPerSecond < right.BitsPerSecond;

    public static bool operator <=(DataRate left, DataRate right) =>
        left.BitsPerSecond <= right.BitsPerSecond;

    public static bool operator >(DataRate left, DataRate right) =>
        left.BitsPerSecond > right.BitsPerSecond;

    public static bool operator >=(DataRate left, DataRate right) =>
        left.BitsPerSecond >= right.BitsPerSecond;

    public int CompareTo(DataRate other) => BitsPerSecond.CompareTo(other.BitsPerSecond);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(BitsPerSecond, Dimension.DataRate, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(BitsPerSecond, Dimension.DataRate, format, formatProvider);

    public override string ToString() => Humanize();
}

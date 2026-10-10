namespace H13y.Measures;

/// <summary>
/// A quantity of digital information measured in bits, used for network bandwidth.
/// </summary>
/// <remarks>
/// This is distinct from <see cref="DataSize"/>, which stores bytes. Bits use 1000-based
/// scaling (Kb = 1000 bits), matching the SI convention used by network equipment.
/// </remarks>
/// <param name="Value">The number of bits.</param>
public readonly record struct Bits(double Value) : IComparable<Bits>, IFormattable
{
    public static Bits FromBits(double bits) => new(bits);

    public static Bits FromKilobits(double kb) => new(kb * 1_000);

    public static Bits FromMegabits(double mb) => new(mb * 1_000_000);

    public static Bits FromGigabits(double gb) => new(gb * 1_000_000_000);

    public static Bits FromTerabits(double tb) => new(tb * 1_000_000_000_000);

    public double ToKilobits() => Value / 1_000;

    public double ToMegabits() => Value / 1_000_000;

    public double ToGigabits() => Value / 1_000_000_000;

    public double ToTerabits() => Value / 1_000_000_000_000;

    public static Bits operator +(Bits left, Bits right) => new(left.Value + right.Value);

    public static Bits operator -(Bits left, Bits right) => new(left.Value - right.Value);

    public static bool operator <(Bits left, Bits right) => left.Value < right.Value;

    public static bool operator <=(Bits left, Bits right) => left.Value <= right.Value;

    public static bool operator >(Bits left, Bits right) => left.Value > right.Value;

    public static bool operator >=(Bits left, Bits right) => left.Value >= right.Value;

    public int CompareTo(Bits other) => Value.CompareTo(other.Value);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(Value, Dimension.Bits, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(Value, Dimension.Bits, format, formatProvider);

    public override string ToString() => Humanize();
}

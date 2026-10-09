namespace H13y.Measures;

/// <summary>
/// A quantity of energy, stored internally as a number of joules.
/// </summary>
/// <param name="Joules">The energy in joules.</param>
public readonly record struct Energy(double Joules) : IComparable<Energy>, IFormattable
{
    public static Energy FromJoules(double j) => new(j);

    public static Energy FromKilojoules(double kj) => new(kj * 1000);

    public static Energy FromMegajoules(double mj) => new(mj * 1_000_000);

    public static Energy FromCalories(double cal) => new(cal * 4.184);

    public static Energy FromKilocalories(double kcal) => new(kcal * 4184);

    public static Energy FromWattHours(double wh) => new(wh * 3600);

    public static Energy FromKilowattHours(double kwh) => new(kwh * 3_600_000);

    public double ToKilojoules() => Joules / 1000;

    public double ToMegajoules() => Joules / 1_000_000;

    public double ToCalories() => Joules / 4.184;

    public double ToKilocalories() => Joules / 4184;

    public double ToWattHours() => Joules / 3600;

    public double ToKilowattHours() => Joules / 3_600_000;

    public static Energy operator +(Energy left, Energy right) => new(left.Joules + right.Joules);

    public static Energy operator -(Energy left, Energy right) => new(left.Joules - right.Joules);

    public static bool operator <(Energy left, Energy right) => left.Joules < right.Joules;

    public static bool operator <=(Energy left, Energy right) => left.Joules <= right.Joules;

    public static bool operator >(Energy left, Energy right) => left.Joules > right.Joules;

    public static bool operator >=(Energy left, Energy right) => left.Joules >= right.Joules;

    public int CompareTo(Energy other) => Joules.CompareTo(other.Joules);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(Joules, Dimension.Energy, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(Joules, Dimension.Energy, format, formatProvider);

    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>
/// A quantity of volume, stored internally as a number of liters.
/// </summary>
/// <param name="Liters">The volume in liters.</param>
public readonly record struct Volume(double Liters) : IComparable<Volume>
{
    public static Volume FromMilliliters(double ml) => new(ml / 1000);
    public static Volume FromCentiliters(double cl) => new(cl / 100);
    public static Volume FromLiters(double l) => new(l);
    public static Volume FromCubicMeters(double m3) => new(m3 * 1000);

    public double ToMilliliters() => Liters * 1000;
    public double ToCentiliters() => Liters * 100;
    public double ToCubicMeters() => Liters / 1000;

    public static Volume operator +(Volume left, Volume right) => new(left.Liters + right.Liters);
    public static Volume operator -(Volume left, Volume right) => new(left.Liters - right.Liters);
    public static bool operator <(Volume left, Volume right) => left.Liters < right.Liters;
    public static bool operator <=(Volume left, Volume right) => left.Liters <= right.Liters;
    public static bool operator >(Volume left, Volume right) => left.Liters > right.Liters;
    public static bool operator >=(Volume left, Volume right) => left.Liters >= right.Liters;

    public int CompareTo(Volume other) => Liters.CompareTo(other.Liters);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Liters, Dimension.Volume, options);

    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>
/// A quantity of length, stored internally as a number of meters.
/// </summary>
/// <param name="Meters">The length in meters.</param>
public readonly record struct Length(double Meters) : IComparable<Length>
{
    public static Length FromMillimeters(double mm) => new(mm / 1000);
    public static Length FromCentimeters(double cm) => new(cm / 100);
    public static Length FromMeters(double m) => new(m);
    public static Length FromKilometers(double km) => new(km * 1000);

    public double ToMillimeters() => Meters * 1000;
    public double ToCentimeters() => Meters * 100;
    public double ToKilometers() => Meters / 1000;

    public static Length operator +(Length left, Length right) => new(left.Meters + right.Meters);
    public static Length operator -(Length left, Length right) => new(left.Meters - right.Meters);
    public static bool operator <(Length left, Length right) => left.Meters < right.Meters;
    public static bool operator <=(Length left, Length right) => left.Meters <= right.Meters;
    public static bool operator >(Length left, Length right) => left.Meters > right.Meters;
    public static bool operator >=(Length left, Length right) => left.Meters >= right.Meters;

    public int CompareTo(Length other) => Meters.CompareTo(other.Meters);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Meters, Dimension.Length, options);

    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>Length, stored as meters.</summary>
public readonly record struct Length(double Meters)
{
    public static Length FromMillimeters(double mm) => new(mm / 1000);
    public static Length FromCentimeters(double cm) => new(cm / 100);
    public static Length FromMeters(double m) => new(m);
    public static Length FromKilometers(double km) => new(km * 1000);

    public double ToMillimeters() => Meters * 1000;
    public double ToCentimeters() => Meters * 100;
    public double ToKilometers() => Meters / 1000;

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Meters, Dimension.Length, options);

    public override string ToString() => Humanize();
}

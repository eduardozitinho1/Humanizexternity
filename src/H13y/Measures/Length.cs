namespace H13y.Measures;

/// <summary>
/// A quantity of length, stored internally as a number of meters.
/// </summary>
/// <remarks>
/// <see cref="Length"/> covers distances and sizes. The base unit is the meter, which is
/// the SI standard and keeps conversions simple for both very small values (millimeters)
/// and large ones (kilometers).
///
/// Only metric units are included in v0.1.0. If you need imperial units (inches, feet,
/// miles), convert at the boundary or wait for a future version that adds them.
/// </remarks>
/// <param name="Meters">The length in meters.</param>
public readonly record struct Length(double Meters)
{
    /// <summary>Creates a <see cref="Length"/> from a number of millimeters.</summary>
    public static Length FromMillimeters(double mm) => new(mm / 1000);

    /// <summary>Creates a <see cref="Length"/> from a number of centimeters.</summary>
    public static Length FromCentimeters(double cm) => new(cm / 100);

    /// <summary>Creates a <see cref="Length"/> from a number of meters.</summary>
    public static Length FromMeters(double m) => new(m);

    /// <summary>Creates a <see cref="Length"/> from a number of kilometers.</summary>
    public static Length FromKilometers(double km) => new(km * 1000);

    /// <summary>Returns the length expressed in millimeters.</summary>
    public double ToMillimeters() => Meters * 1000;

    /// <summary>Returns the length expressed in centimeters.</summary>
    public double ToCentimeters() => Meters * 100;

    /// <summary>Returns the length expressed in kilometers.</summary>
    public double ToKilometers() => Meters / 1000;

    /// <summary>
    /// Renders the length as a human-readable string such as "1.5 km" or "5 mm".
    /// </summary>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Meters, Dimension.Length, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>
/// A quantity of area, stored internally as a number of square meters.
/// </summary>
/// <remarks>
/// <see cref="Area"/> represents surfaces and land. The base unit is the square meter,
/// with hectares included for land measurement (1 ha = 10,000 m²).
/// </remarks>
/// <param name="SquareMeters">The area in square meters.</param>
public readonly record struct Area(double SquareMeters)
{
    /// <summary>Creates an <see cref="Area"/> from a number of square millimeters.</summary>
    public static Area FromSquareMillimeters(double mm2) => new(mm2 / 1_000_000);

    /// <summary>Creates an <see cref="Area"/> from a number of square centimeters.</summary>
    public static Area FromSquareCentimeters(double cm2) => new(cm2 / 10_000);

    /// <summary>Creates an <see cref="Area"/> from a number of square meters.</summary>
    public static Area FromSquareMeters(double m2) => new(m2);

    /// <summary>Creates an <see cref="Area"/> from a number of hectares.</summary>
    public static Area FromHectares(double ha) => new(ha * 10_000);

    /// <summary>Creates an <see cref="Area"/> from a number of square kilometers.</summary>
    public static Area FromSquareKilometers(double km2) => new(km2 * 1_000_000);

    /// <summary>Returns the area expressed in square millimeters.</summary>
    public double ToSquareMillimeters() => SquareMeters * 1_000_000;

    /// <summary>Returns the area expressed in square centimeters.</summary>
    public double ToSquareCentimeters() => SquareMeters * 10_000;

    /// <summary>Returns the area expressed in hectares.</summary>
    public double ToHectares() => SquareMeters / 10_000;

    /// <summary>Returns the area expressed in square kilometers.</summary>
    public double ToSquareKilometers() => SquareMeters / 1_000_000;

    /// <summary>Renders the area as a human-readable string such as "1.5 ha" or "500 m2".</summary>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(SquareMeters, Dimension.Area, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

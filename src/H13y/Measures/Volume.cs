namespace H13y.Measures;

/// <summary>
/// A quantity of volume, stored internally as a number of liters.
/// </summary>
/// <remarks>
/// <see cref="Volume"/> represents capacities and amounts of liquid or gas. The base unit
/// is the liter, which is intuitive for everyday use (bottles, tanks) while still
/// supporting scientific small values through milliliters and industrial volumes through
/// cubic meters.
///
/// The stored value is a <see cref="double"/>, so fractional volumes are preserved exactly
/// as provided.
/// </remarks>
/// <param name="Liters">The volume in liters.</param>
public readonly record struct Volume(double Liters)
{
    /// <summary>Creates a <see cref="Volume"/> from a number of milliliters.</summary>
    public static Volume FromMilliliters(double ml) => new(ml / 1000);

    /// <summary>Creates a <see cref="Volume"/> from a number of centiliters.</summary>
    public static Volume FromCentiliters(double cl) => new(cl / 100);

    /// <summary>Creates a <see cref="Volume"/> from a number of liters.</summary>
    public static Volume FromLiters(double l) => new(l);

    /// <summary>Creates a <see cref="Volume"/> from a number of cubic meters.</summary>
    public static Volume FromCubicMeters(double m3) => new(m3 * 1000);

    /// <summary>Returns the volume expressed in milliliters.</summary>
    public double ToMilliliters() => Liters * 1000;

    /// <summary>Returns the volume expressed in centiliters.</summary>
    public double ToCentiliters() => Liters * 100;

    /// <summary>Returns the volume expressed in cubic meters.</summary>
    public double ToCubicMeters() => Liters / 1000;

    /// <summary>
    /// Renders the volume as a human-readable string such as "1.5 l" or "500 ml".
    /// </summary>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Liters, Dimension.Volume, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

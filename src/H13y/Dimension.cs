namespace H13y;

/// <summary>
/// Identifies the physical dimension (family) a unit belongs to.
/// </summary>
/// <remarks>
/// Units within the same dimension can be converted between each other, because they
/// share the same base unit. Units from different dimensions are incompatible.
/// </remarks>
public enum Dimension
{
    /// <summary>Digital information. Base unit: byte.</summary>
    Data,

    /// <summary>Mass. Base unit: gram.</summary>
    Mass,

    /// <summary>Length. Base unit: meter.</summary>
    Length,

    /// <summary>Time. Base unit: second.</summary>
    Time,

    /// <summary>Volume. Base unit: liter.</summary>
    Volume,

    /// <summary>Area. Base unit: square meter.</summary>
    Area,

    /// <summary>
    /// Temperature. Base unit: kelvin. Unlike other dimensions, temperature units are affine
    /// (they carry an additive offset), which means auto unit selection does not apply.
    /// Use <c>Temperature.Humanize</c> or <c>TemperatureFormatter.Format</c> for display.
    /// </summary>
    Temperature,
}

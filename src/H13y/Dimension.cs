namespace H13y;

/// <summary>
/// Identifies the physical dimension (family) a unit belongs to.
/// </summary>
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

    /// <summary>Temperature. Base unit: kelvin. Affine, so auto unit selection does not apply.</summary>
    Temperature,

    /// <summary>Speed. Base unit: meter per second.</summary>
    Speed,

    /// <summary>Energy. Base unit: joule.</summary>
    Energy,

    /// <summary>Power. Base unit: watt.</summary>
    Power,

    /// <summary>Pressure. Base unit: pascal.</summary>
    Pressure,

    /// <summary>Frequency. Base unit: hertz.</summary>
    Frequency,

    /// <summary>Angle. Base unit: radian.</summary>
    Angle,
}

namespace H13y;

/// <summary>
/// Identifies the physical dimension (family) a unit belongs to.
/// </summary>
/// <remarks>
/// A dimension is a category of measurement such as data, mass, length, time, volume,
/// or area. Units within the same dimension can be converted between each other, because
/// they share the same base unit. Units from different dimensions are incompatible.
///
/// This enum is the key that <see cref="BestUnitSelector"/> uses to find the most
/// appropriate unit when calling <see cref="H.Best"/> or <see cref="HumanizeFormatter.Format"/>.
/// </remarks>
public enum Dimension
{
    /// <summary>Digital information. Base unit: byte. Units: B, KB, MB, GB, TB, PB.</summary>
    Data,

    /// <summary>Mass. Base unit: gram. Units: mg, g, kg, t.</summary>
    Mass,

    /// <summary>Length. Base unit: meter. Units: mm, cm, m, km.</summary>
    Length,

    /// <summary>Time. Base unit: second. Units: ms, s, min, h, d, w.</summary>
    Time,

    /// <summary>Volume. Base unit: liter. Units: ml, cl, l, m3.</summary>
    Volume,

    /// <summary>Area. Base unit: square meter. Units: mm2, cm2, m2, km2, ha.</summary>
    Area,
}

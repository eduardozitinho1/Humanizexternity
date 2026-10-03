namespace H13y;

/// <summary>
/// Identifies the physical dimension (family) a unit belongs to.
/// </summary>
/// <remarks>
/// A dimension is a category of measurement such as data, mass, length, or time.
/// Units within the same dimension can be converted between each other, because
/// they share the same base unit (byte, gram, meter, second). Units from different
/// dimensions are incompatible and cannot be mixed.
///
/// This enum is the key that <see cref="BestUnitSelector"/> uses to find the most
/// appropriate unit when calling <see cref="H.Best"/> or <see cref="HumanizeFormatter.Format"/>.
/// New dimensions are added here as the library grows (Volume, Area, Speed, Temperature, etc.).
/// </remarks>
public enum Dimension
{
    /// <summary>
    /// Digital information. Base unit: byte. Units include B, KB, MB, GB, TB, PB.
    /// </summary>
    Data,

    /// <summary>
    /// Mass. Base unit: gram. Units include mg, g, kg, t.
    /// </summary>
    Mass,

    /// <summary>
    /// Length. Base unit: meter. Units include mm, cm, m, km.
    /// </summary>
    Length,

    /// <summary>
    /// Time. Base unit: second. Units include ms, s, min, h, d, w.
    /// </summary>
    Time,
}

namespace H13y;

/// <summary>
/// Represents a physical dimension (kind of measure).
/// </summary>
public enum Dimension
{
    /// <summary>Digital information (B, KB, MB, GB, TB, PB).</summary>
    Data,

    /// <summary>Mass (mg, g, kg, t).</summary>
    Mass,

    /// <summary>Length (mm, cm, m, km).</summary>
    Length,

    /// <summary>Time (ms, s, min, h, d, w).</summary>
    Time,
}

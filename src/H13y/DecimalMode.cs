namespace H13y;

/// <summary>
/// Determines the factor used to scale data units.
/// </summary>
public enum DecimalMode
{
    /// <summary>1024-based (1 KB = 1024 B). Default for data.</summary>
    Binary,

    /// <summary>1000-based (1 kB = 1000 B). SI standard.</summary>
    Metric,
}

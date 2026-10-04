using System.Globalization;

namespace H13y;

/// <summary>
/// Controls how values are turned into human-readable strings.
/// </summary>
public sealed record HumanizeOptions
{
    /// <summary>Default options: binary scaling, invariant culture, one decimal, space between value and unit.</summary>
    public static HumanizeOptions Default { get; } = new();

    /// <summary>Whether data units use 1024-based (binary) or 1000-based (metric) scaling.</summary>
    public DecimalMode Mode { get; init; } = DecimalMode.Binary;

    /// <summary>Culture used when formatting numbers.</summary>
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;

    /// <summary>Maximum number of decimal places to show. Default is 1.</summary>
    public int MaxDecimals { get; init; } = 1;

    /// <summary>Whether to insert a space between the numeric value and the unit symbol.</summary>
    public bool SpaceBetweenValueAndUnit { get; init; } = true;

    /// <summary>
    /// Whether data units display IEC symbols (KiB, MiB, GiB, TiB, PiB) instead of the
    /// traditional KB, MB, GB, TB, PB. The numeric factor stays 1024 in both cases.
    /// </summary>
    public bool UseIecSymbols { get; init; }
}

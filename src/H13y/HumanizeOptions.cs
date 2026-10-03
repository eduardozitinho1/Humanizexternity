using System.Globalization;

namespace H13y;

/// <summary>
/// Options that control how values are humanized.
/// </summary>
public sealed record HumanizeOptions
{
    /// <summary>Default options: Binary, InvariantCulture, 1 decimal, space between value and unit.</summary>
    public static HumanizeOptions Default { get; } = new();

    /// <summary>Binary (1024) or Decimal (1000) scaling for data units. Default: Binary.</summary>
    public DecimalMode Mode { get; init; } = DecimalMode.Binary;

    /// <summary>Culture used for number formatting. Default: InvariantCulture.</summary>
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;

    /// <summary>Maximum number of decimal places. Default: 1.</summary>
    public int MaxDecimals { get; init; } = 1;

    /// <summary>Whether to put a space between the value and the unit. Default: true.</summary>
    public bool SpaceBetweenValueAndUnit { get; init; } = true;
}

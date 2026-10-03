using System.Globalization;

namespace H13y;

/// <summary>
/// Controls how values are turned into human-readable strings.
/// </summary>
/// <remarks>
/// Every humanization method in the library accepts an optional <see cref="HumanizeOptions"/>
/// instance. When omitted, <see cref="Default"/> is used, which provides sensible behavior
/// for most applications: binary scaling for data, invariant culture, one decimal place,
/// and a space between the value and the unit.
///
/// The type is an immutable <c>record</c>, so you can safely share instances and derive
/// new ones using <c>with</c> expressions. Example:
///
/// <code>
/// var compact = HumanizeOptions.Default with { SpaceBetweenValueAndUnit = false };
/// H.Bytes(1536, compact); // "1.5KB"
/// </code>
/// </remarks>
public sealed record HumanizeOptions
{
    /// <summary>
    /// Default options: binary scaling, invariant culture, one decimal, space between value and unit.
    /// </summary>
    public static HumanizeOptions Default { get; } = new();

    /// <summary>
    /// Whether data units use 1024-based (binary) or 1000-based (metric) scaling.
    /// </summary>
    /// <remarks>
    /// This option only affects the <see cref="Dimension.Data"/> dimension. Other dimensions
    /// are unaffected. Default is <see cref="DecimalMode.Binary"/>.
    /// </remarks>
    public DecimalMode Mode { get; init; } = DecimalMode.Binary;

    /// <summary>
    /// Culture used when formatting numbers, controlling the decimal separator and digit grouping.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="CultureInfo.InvariantCulture"/>, which uses a dot as the decimal
    /// separator. Set to a specific culture such as <c>pt-BR</c> to get locale-aware output
    /// (for example, "1,5 KB" instead of "1.5 KB").
    /// </remarks>
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Maximum number of decimal places to show. Default is 1.
    /// </summary>
    /// <remarks>
    /// Trailing zeros are not shown, so a value like <c>2.0</c> with <see cref="MaxDecimals"/> = 2
    /// renders as "2", while <c>2.5</c> renders as "2.5". Set to 0 to force integer output.
    /// </remarks>
    public int MaxDecimals { get; init; } = 1;

    /// <summary>
    /// Whether to insert a space between the numeric value and the unit symbol. Default is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, output looks like "1 GB". When <c>false</c>, output looks like "1GB".
    /// The compact style is useful in dense UIs (tables, badges) where space is limited.
    /// </remarks>
    public bool SpaceBetweenValueAndUnit { get; init; } = true;
}

using System.Globalization;
using Microsoft.Extensions.Localization;
 
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
/// <summary>
/// Whether unit labels are rendered as compact symbols ("kg", "GB") or as
/// full English names ("kilogram", "gigabyte"). Default is <see cref="UnitStyle.Symbol"/>.
/// </summary>
public UnitStyle UnitStyle { get; init; } = UnitStyle.Symbol;

/// <summary>
/// Whether unit names are pluralized based on the value when
/// <see cref="UnitStyle"/> is <see cref="UnitStyle.FullName"/>.
/// Default is <c>true</c>. Has no effect when the style is <see cref="UnitStyle.Symbol"/>.
/// </summary>
public bool Pluralize { get; init; } = true;

/// <summary>
/// Optional <see cref="IStringLocalizer"/> used to translate unit names and
/// relative-time phrases. When null, the built-in English strings are used.
/// </summary>
/// <remarks>
/// See <see cref="Localization.LocalizationKeys"/> for the key conventions.
/// Missing keys fall back to the built-in English string, so partial
/// localization is fully valid.
/// </remarks>
public IStringLocalizer? Localizer { get; init; }
}

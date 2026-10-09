namespace H13y;

/// <summary>
/// Determines whether unit labels are rendered as their compact symbol
/// (for example "kg" or "GB") or as a full English name
/// (for example "kilogram" or "gigabyte").
/// </summary>
/// <remarks>
/// The default is <see cref="Symbol"/>, which preserves the output produced by
/// earlier versions of the library. Full names are English-only and target
/// user-facing text where readability matters more than compactness.
/// Pluralization is controlled separately by <see cref="HumanizeOptions.Pluralize"/>.
/// </remarks>
public enum UnitStyle
{
    /// <summary>Compact unit symbol, such as "kg" or "GB". Default.</summary>
    Symbol = 0,

    /// <summary>Full unit name, such as "kilogram" or "gigabyte".</summary>
    FullName = 1,
}

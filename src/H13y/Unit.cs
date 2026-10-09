namespace H13y;

/// <summary>
/// Represents a unit of measure, such as "KB", "kg", "m", or "s".
/// </summary>
/// <remarks>
/// Conversion to the base unit is <c>base = value * Factor + Offset</c>, and the inverse is
/// <c>value = (base - Offset) / Factor</c>. For linear dimensions <see cref="Offset"/> is zero.
/// </remarks>
/// <param name="Symbol">The display symbol, such as "KB", "kg", "m", "°C".</param>
/// <param name="Factor">Multiplier to convert this unit to the dimension's base unit.</param>
/// <param name="Dimension">The dimension this unit belongs to.</param>
/// <param name="Offset">Additive constant applied after the factor. Zero for linear units.</param>
/// <param name="IecSymbol">Alternative IEC symbol used for data units when <see cref="HumanizeOptions.UseIecSymbols"/> is enabled. Null for non-data units.</param>
public sealed record Unit(
    string Symbol,
    double Factor,
    Dimension Dimension,
    double Offset = 0,
    string? IecSymbol = null
)
{
    /// <summary>
    /// Returns the symbol to display, honoring the IEC preference.
    /// </summary>
    /// <param name="useIec">When true and <see cref="IecSymbol"/> is set, returns it; otherwise returns <see cref="Symbol"/>.</param>
    public string DisplaySymbol(bool useIec) =>
        useIec && IecSymbol is not null ? IecSymbol : Symbol;

    /// <summary>Returns the unit's <see cref="Symbol"/> so it prints cleanly in logs.</summary>
    public override string ToString() => Symbol;
}

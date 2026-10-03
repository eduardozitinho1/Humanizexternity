namespace H13y;

/// <summary>
/// Represents a unit of measure, such as "KB", "kg", "m", or "s".
/// </summary>
/// <remarks>
/// A unit carries four pieces of information:
///
/// <list type="bullet">
///   <item><description><see cref="Symbol"/> — the short text shown to users (e.g. "kg").</description></item>
///   <item><description><see cref="Factor"/> — the multiplier applied to convert to the base unit.</description></item>
///   <item><description><see cref="Dimension"/> — the family this unit belongs to.</description></item>
///   <item><description><see cref="Offset"/> — the additive constant for affine units such as temperature. Zero for all linear units.</description></item>
/// </list>
///
/// Conversion to the base unit is <c>base = value * Factor + Offset</c>, and the inverse is
/// <c>value = (base - Offset) / Factor</c>. For linear dimensions (mass, length, data, time,
/// volume, area) <see cref="Offset"/> is zero and the formula reduces to a simple ratio. For
/// temperature the offset is what makes the affine conversion correct.
/// </remarks>
/// <param name="Symbol">The display symbol, such as "KB", "kg", "m", "°C".</param>
/// <param name="Factor">Multiplier to convert this unit to the dimension's base unit.</param>
/// <param name="Dimension">The dimension this unit belongs to.</param>
/// <param name="Offset">Additive constant applied after the factor. Zero for linear units.</param>
public sealed record Unit(string Symbol, double Factor, Dimension Dimension, double Offset = 0)
{
    /// <summary>Returns the unit's <see cref="Symbol"/> so it prints cleanly in logs.</summary>
    public override string ToString() => Symbol;
}

namespace H13y;

/// <summary>
/// Represents a unit of measure, such as "KB", "kg", "m", or "s".
/// </summary>
/// <remarks>
/// A unit carries three pieces of information:
///
/// <list type="bullet">
///   <item><description><see cref="Symbol"/> — the short text shown to users (e.g. "kg").</description></item>
///   <item><description><see cref="Factor"/> — how many base units equal one of this unit. For "kg" in the Mass dimension, the factor is 1000 because 1 kg = 1000 g.</description></item>
///   <item><description><see cref="Dimension"/> — the family this unit belongs to, so the library knows which other units it can convert against.</description></item>
/// </list>
///
/// Instances are immutable records, so they can be safely shared, compared by value,
/// and used as dictionary keys. The library registers all built-in units in
/// <see cref="Units"/>, and the parser resolves user input to these instances
/// through <see cref="UnitAliases"/>.
/// </remarks>
/// <param name="Symbol">The display symbol, such as "KB", "kg", "m", or "s".</param>
/// <param name="Factor">Multiplier to convert this unit to the dimension's base unit.</param>
/// <param name="Dimension">The dimension this unit belongs to.</param>
public sealed record Unit(string Symbol, double Factor, Dimension Dimension)
{
    /// <summary>
    /// Returns the unit's <see cref="Symbol"/> so it prints cleanly in logs and interpolated strings.
    /// </summary>
    /// <returns>The unit symbol, for example <c>"kg"</c>.</returns>
    public override string ToString() => Symbol;
}

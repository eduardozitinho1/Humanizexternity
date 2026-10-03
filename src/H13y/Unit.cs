namespace H13y;

/// <summary>
/// Represents a unit of measure.
/// </summary>
/// <param name="Symbol">Symbol like "KB", "kg", "m", "s".</param>
/// <param name="Factor">Factor to convert to the base unit of the dimension.</param>
/// <param name="Dimension">The dimension this unit belongs to.</param>
public sealed record Unit(string Symbol, double Factor, Dimension Dimension)
{
    public override string ToString() => Symbol;
}

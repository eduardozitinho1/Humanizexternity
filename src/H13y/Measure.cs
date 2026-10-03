namespace H13y;

/// <summary>
/// A value paired with a unit of measure.
/// </summary>
public readonly record struct Measure(double Value, Unit Unit)
{
    /// <summary>Converts the value to the dimension's base unit.</summary>
    public double ToBase() => Value * Unit.Factor;
}

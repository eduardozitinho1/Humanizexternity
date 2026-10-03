namespace H13y;

/// <summary>
/// A numeric value paired with a <see cref="Unit"/>, produced by parsing human-readable input.
/// </summary>
/// <remarks>
/// A <see cref="Measure"/> keeps the value exactly as the user typed it, together with
/// the unit they used. It does <b>not</b> normalize to the base unit automatically —
/// call <see cref="ToBase"/> when you need the value expressed in the dimension's base
/// unit (bytes, grams, meters, seconds, liters, square meters, kelvin).
/// </remarks>
/// <param name="Value">The numeric amount the user provided.</param>
/// <param name="Unit">The unit the amount is expressed in.</param>
public readonly record struct Measure(double Value, Unit Unit)
{
    /// <summary>
    /// Converts the value to the base unit of its dimension using <c>value * factor + offset</c>.
    /// </summary>
    public double ToBase() => Value * Unit.Factor + Unit.Offset;
}

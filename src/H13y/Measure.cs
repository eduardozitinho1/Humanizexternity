namespace H13y;

/// <summary>
/// A numeric value paired with a <see cref="Unit"/>, produced by parsing human-readable input.
/// </summary>
/// <remarks>
/// A <see cref="Measure"/> keeps the value exactly as the user typed it, together with
/// the unit they used. It does <b>not</b> normalize to the base unit automatically —
/// call <see cref="ToBase"/> when you need the value expressed in the dimension's base
/// unit (bytes, grams, meters, seconds).
///
/// This is the type returned by <see cref="H.Parse"/> and <see cref="UnitParser.Parse"/>.
/// It is a value type (<c>record struct</c>), so it is cheap to copy and compare.
/// </remarks>
/// <param name="Value">The numeric amount the user provided.</param>
/// <param name="Unit">The unit the amount is expressed in.</param>
public readonly record struct Measure(double Value, Unit Unit)
{
    /// <summary>
    /// Converts the value to the base unit of its dimension.
    /// </summary>
    /// <returns>
    /// The value multiplied by <see cref="Unit.Factor"/>. For example, a measure of
    /// <c>1.5 kg</c> returns <c>1500</c> because the base unit of mass is the gram.
    /// </returns>
    public double ToBase() => Value * Unit.Factor;
}

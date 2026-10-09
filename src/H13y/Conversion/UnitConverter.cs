namespace H13y;

/// <summary>
/// Converts a numeric value from one unit to another within the same dimension.
/// </summary>
/// <remarks>
/// Conversion is done through the dimension's base unit using
/// <c>base = value * from.Factor + from.Offset</c> and
/// <c>result = (base - to.Offset) / to.Factor</c>.
///
/// This works for linear dimensions (mass, length, data, time, volume, area) because
/// <see cref="Unit.Offset"/> is zero for all their units. It also works for affine
/// dimensions such as temperature because the offset is carried through the calculation.
/// </remarks>
public static class UnitConverter
{
    /// <summary>
    /// Converts <paramref name="value"/> from <paramref name="from"/> to <paramref name="to"/>.
    /// </summary>
    /// <param name="value">The numeric value expressed in <paramref name="from"/>.</param>
    /// <param name="from">The source unit.</param>
    /// <param name="to">The target unit.</param>
    /// <returns>The value expressed in <paramref name="to"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either unit is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the units belong to different dimensions.</exception>
    public static double Convert(double value, Unit from, Unit to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Dimension != to.Dimension)
            throw new ArgumentException(
                $"Cannot convert between '{from.Symbol}' ({from.Dimension}) and '{to.Symbol}' ({to.Dimension})."
            );

        var baseValue = value * from.Factor + from.Offset;
        return (baseValue - to.Offset) / to.Factor;
    }
}

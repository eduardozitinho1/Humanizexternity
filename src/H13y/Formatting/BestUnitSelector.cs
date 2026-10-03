namespace H13y;

/// <summary>
/// Chooses the most appropriate unit to display a value in, given its dimension.
/// </summary>
/// <remarks>
/// The selection follows a simple rule: from the largest candidate unit to the smallest,
/// return the first one whose factor is less than or equal to the base value. This means
/// a value of 1536 bytes is shown as "1.5 KB" (because 1536 ≥ 1024) rather than "1536 B"
/// (which would be correct but less readable), and a value of 1073741824 bytes is shown
/// as "1 GB" (the largest unit that still fits).
///
/// If no unit fits (the value is smaller than the smallest unit), the smallest unit is
/// returned so the value still renders with a real symbol instead of throwing. This is
/// what allows, for example, a fraction of a gram to display as "0.5 g" rather than fail.
/// </remarks>
internal static class BestUnitSelector
{
    /// <summary>
    /// Picks the best unit for the given base value.
    /// </summary>
    /// <param name="baseValue">The value expressed in the dimension's base unit.</param>
    /// <param name="dimension">The dimension whose units should be considered.</param>
    /// <returns>The most appropriate <see cref="Unit"/> for the value.</returns>
    /// <exception cref="ArgumentException">Thrown when no units are registered for the dimension.</exception>
    public static Unit Pick(double baseValue, Dimension dimension)
    {
        var candidates = Units.ByDimension(dimension)
            .OrderByDescending(u => u.Factor)
            .ToArray();

        if (candidates.Length == 0)
            throw new ArgumentException($"No units registered for dimension '{dimension}'.", nameof(dimension));

        foreach (var unit in candidates)
        {
            if (baseValue >= unit.Factor)
                return unit;
        }

        return candidates[^1];
    }
}

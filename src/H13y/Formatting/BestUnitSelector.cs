namespace H13y;

/// <summary>
/// Chooses the most appropriate unit to display a value in, given its dimension.
/// </summary>
/// <remarks>
/// The selection follows a simple rule: from the largest candidate unit to the smallest,
/// return the first one whose factor is less than or equal to the base value.
///
/// Two special cases are handled explicitly:
///
/// <list type="bullet">
///   <item><description><b>Zero</b> — returns the base unit (factor 1) so that "0 g", "0 m", and "0 l" render naturally instead of falling through to the smallest unit like "0 mg" or "0 mm".</description></item>
///   <item><description><b>Below the smallest unit</b> — returns the smallest unit so fractional values like 0.5 g still display with a real symbol.</description></item>
/// </list>
/// </remarks>
internal static class BestUnitSelector
{
    public static Unit Pick(double baseValue, Dimension dimension)
    {
        var candidates = Units.ByDimension(dimension)
            .OrderByDescending(u => u.Factor)
            .ToArray();

        if (candidates.Length == 0)
            throw new ArgumentException($"No units registered for dimension '{dimension}'.", nameof(dimension));

        if (baseValue == 0)
        {
            var baseUnit = candidates.FirstOrDefault(u => u.Factor == 1);
            return baseUnit ?? candidates[^1];
        }

        foreach (var unit in candidates)
        {
            if (baseValue >= unit.Factor)
                return unit;
        }

        return candidates[^1];
    }
}

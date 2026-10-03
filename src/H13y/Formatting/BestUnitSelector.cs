namespace H13y;

/// <summary>
/// Picks the most appropriate unit for a given value in base units.
/// </summary>
internal static class BestUnitSelector
{
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

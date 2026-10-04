namespace H13y;

/// <summary>
/// Chooses the most appropriate unit to display a value in, given its dimension.
/// </summary>
/// <remarks>
/// The candidate list for each dimension is precomputed and sorted by descending factor,
/// so repeated calls in formatting loops do not sort or allocate.
/// </remarks>
internal static class BestUnitSelector
{
    private static readonly Dictionary<Dimension, Unit[]> DescendingCache = BuildCache();

    public static Unit Pick(double baseValue, Dimension dimension)
    {
        if (dimension == Dimension.Temperature)
            throw new ArgumentException(
                "Dimension.Temperature is affine, not linear. Use Temperature.Humanize or TemperatureFormatter.Format.",
                nameof(dimension));

        if (!DescendingCache.TryGetValue(dimension, out var candidates) || candidates.Length == 0)
            throw new ArgumentException($"No units registered for dimension '{dimension}'.", nameof(dimension));

        if (baseValue == 0)
        {
            foreach (var unit in candidates)
                if (unit.Factor == 1) return unit;

            return candidates[^1];
        }

        foreach (var unit in candidates)
        {
            if (baseValue >= unit.Factor)
                return unit;
        }

        return candidates[^1];
    }

    private static Dictionary<Dimension, Unit[]> BuildCache()
        => Units.All
            .GroupBy(u => u.Dimension)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(u => u.Factor).ToArray());
}

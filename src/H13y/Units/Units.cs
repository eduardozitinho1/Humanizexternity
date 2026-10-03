namespace H13y;

/// <summary>
/// Central registry of every unit known to the library.
/// </summary>
/// <remarks>
/// This class aggregates all units from every dimension into a single list, so
/// <see cref="BestUnitSelector"/> and <see cref="UnitAliases"/> have one place to look.
/// The registry is built once at type initialization and never mutated afterwards,
/// which makes it safe to enumerate from multiple threads.
///
/// To add a new dimension, create a new partial file next to the existing ones
/// (for example, <c>VolumeUnits.cs</c>) and append its units to <see cref="AllUnits"/>.
/// The library intentionally keeps additions here so third-party extensions can
/// follow the same pattern.
/// </remarks>
public static partial class Units
{
    private static readonly Unit[] AllUnits =
    [
        .. Data.All,
        .. Mass.All,
        .. Length.All,
        .. Time.All,
    ];

    /// <summary>
    /// Gets every unit registered in the library, across all dimensions.
    /// </summary>
    public static IReadOnlyList<Unit> All => AllUnits;

    /// <summary>
    /// Enumerates all units that belong to a given dimension.
    /// </summary>
    /// <param name="dimension">The dimension to filter by.</param>
    /// <returns>An enumerable of units whose <see cref="Unit.Dimension"/> matches <paramref name="dimension"/>.</returns>
    public static IEnumerable<Unit> ByDimension(Dimension dimension)
        => AllUnits.Where(u => u.Dimension == dimension);
}

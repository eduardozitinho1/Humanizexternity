namespace H13y;

/// <summary>
/// Central registry of every unit known to the library.
/// </summary>
/// <remarks>
/// This class aggregates all units from every dimension into a single list, so
/// <see cref="BestUnitSelector"/> and <see cref="UnitAliases"/> have one place to look.
/// The registry is built once at type initialization and never mutated afterwards,
/// which makes it safe to enumerate from multiple threads.
/// </remarks>
public static partial class Units
{
    private static readonly Unit[] AllUnits =
    [
        .. Data.All,
        .. Mass.All,
        .. Length.All,
        .. Time.All,
        .. Volume.All,
        .. Area.All,
        .. Temperature.All,
    ];

    /// <summary>Gets every unit registered in the library, across all dimensions.</summary>
    public static IReadOnlyList<Unit> All => AllUnits;

    /// <summary>Enumerates all units that belong to a given dimension.</summary>
    public static IEnumerable<Unit> ByDimension(Dimension dimension)
        => AllUnits.Where(u => u.Dimension == dimension);
}

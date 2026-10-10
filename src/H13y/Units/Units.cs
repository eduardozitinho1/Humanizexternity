namespace H13y;

/// <summary>
/// Central registry of every unit known to the library.
/// </summary>
/// <remarks>
/// The full list is built once at type initialization and never mutated.
/// A per-dimension lookup is also precomputed so hot paths avoid LINQ on every call.
/// </remarks>
public static partial class Units
{
    private static readonly Unit[] AllUnits =
    [
        .. Data.All,
        .. Bits.All,
        .. Mass.All,
        .. Length.All,
        .. Time.All,
        .. Volume.All,
        .. Area.All,
        .. Temperature.All,
        .. Speed.All,
        .. Energy.All,
        .. Power.All,
        .. Pressure.All,
        .. Frequency.All,
        .. Angle.All,
    ];

    private static readonly Dictionary<Dimension, Unit[]> ByDimensionMap = BuildMap();

    /// <summary>Gets every unit registered in the library, across all dimensions.</summary>
    public static IReadOnlyList<Unit> All => AllUnits;

    /// <summary>
    /// Enumerates all units that belong to a given dimension.
    /// </summary>
    /// <remarks>
    /// The result is a precomputed array, so this call is allocation-free.
    /// </remarks>
    public static IReadOnlyList<Unit> ByDimension(Dimension dimension) =>
        ByDimensionMap.TryGetValue(dimension, out var units) ? units : Array.Empty<Unit>();

    private static Dictionary<Dimension, Unit[]> BuildMap() =>
        AllUnits.GroupBy(u => u.Dimension).ToDictionary(g => g.Key, g => g.ToArray());
}

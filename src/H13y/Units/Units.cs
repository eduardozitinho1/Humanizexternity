namespace H13y;

/// <summary>
/// Registry of all known units.
/// </summary>
public static partial class Units
{
    private static readonly Unit[] AllUnits =
    [
        .. Data.All,
        .. Mass.All,
        .. Length.All,
        .. Time.All,
    ];

    /// <summary>All registered units.</summary>
    public static IReadOnlyList<Unit> All => AllUnits;

    /// <summary>All units of a given dimension.</summary>
    public static IEnumerable<Unit> ByDimension(Dimension dimension)
        => AllUnits.Where(u => u.Dimension == dimension);
}

namespace H13y;

/// <summary>
/// Central registry of every unit known to the library.
/// </summary>
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
        .. Speed.All,
        .. Energy.All,
        .. Power.All,
        .. Pressure.All,
        .. Frequency.All,
        .. Angle.All,
    ];

    /// <summary>Gets every unit registered in the library, across all dimensions.</summary>
    public static IReadOnlyList<Unit> All => AllUnits;

    /// <summary>Enumerates all units that belong to a given dimension.</summary>
    public static IEnumerable<Unit> ByDimension(Dimension dimension)
        => AllUnits.Where(u => u.Dimension == dimension);
}

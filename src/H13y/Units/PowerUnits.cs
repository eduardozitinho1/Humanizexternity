namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Power units. Base: watt (W).
    /// </summary>
    public static class Power
    {
        /// <summary>Watt — the base unit of power.</summary>
        public static readonly Unit Watt = new("W", 1, Dimension.Power);

        /// <summary>Kilowatt — 1000 watts.</summary>
        public static readonly Unit Kilowatt = new("kW", 1000, Dimension.Power);

        /// <summary>Megawatt — 1,000,000 watts.</summary>
        public static readonly Unit Megawatt = new("MW", 1_000_000, Dimension.Power);

        /// <summary>Gigawatt — 1,000,000,000 watts.</summary>
        public static readonly Unit Gigawatt = new("GW", 1_000_000_000, Dimension.Power);

        /// <summary>Mechanical horsepower — 745.7 watts.</summary>
        public static readonly Unit Horsepower = new("hp", 745.7, Dimension.Power);

        internal static readonly Unit[] All =
        [
            Watt, Kilowatt, Megawatt, Gigawatt,
        ];
    }
}

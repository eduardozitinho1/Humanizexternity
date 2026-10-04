namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Energy units. Base: joule (J).
    /// </summary>
    /// <remarks>
    /// Auto unit selection considers the SI chain (J, kJ, MJ) plus kWh, which is
    /// what people actually read. Calorie, kilocalorie, and watt-hour remain
    /// available through the parser and <see cref="H.Convert"/>.
    /// </remarks>
    public static class Energy
    {
        /// <summary>Joule — the base unit of energy.</summary>
        public static readonly Unit Joule = new("J", 1, Dimension.Energy);

        /// <summary>Kilojoule — 1000 joules.</summary>
        public static readonly Unit Kilojoule = new("kJ", 1000, Dimension.Energy);

        /// <summary>Megajoule — 1,000,000 joules.</summary>
        public static readonly Unit Megajoule = new("MJ", 1_000_000, Dimension.Energy);

        /// <summary>Calorie — the thermochemical calorie, 4.184 joules.</summary>
        public static readonly Unit Calorie = new("cal", 4.184, Dimension.Energy);

        /// <summary>Kilocalorie — 4184 joules (the "food calorie").</summary>
        public static readonly Unit Kilocalorie = new("kcal", 4184, Dimension.Energy);

        /// <summary>Watt-hour — 3600 joules.</summary>
        public static readonly Unit WattHour = new("Wh", 3600, Dimension.Energy);

        /// <summary>Kilowatt-hour — 3,600,000 joules.</summary>
        public static readonly Unit KilowattHour = new("kWh", 3_600_000, Dimension.Energy);

        internal static readonly Unit[] All =
        [
            Joule, Kilojoule, Megajoule, KilowattHour,
        ];
    }
}

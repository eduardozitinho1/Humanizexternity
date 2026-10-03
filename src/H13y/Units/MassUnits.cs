namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Mass units, from milligrams up to tonnes.
    /// </summary>
    /// <remarks>
    /// The base unit is the gram (<see cref="Gram"/>), which keeps the numbers reasonable
    /// for everyday measurements while still supporting very small and very large values
    /// through the appropriate prefix. All factors are powers of ten, matching the SI system.
    /// </remarks>
    public static class Mass
    {
        /// <summary>Milligram — one thousandth of a gram.</summary>
        public static readonly Unit Milligram = new("mg", 0.001, Dimension.Mass);

        /// <summary>Gram — the base unit of mass.</summary>
        public static readonly Unit Gram = new("g", 1, Dimension.Mass);

        /// <summary>Kilogram — 1000 grams.</summary>
        public static readonly Unit Kilogram = new("kg", 1000, Dimension.Mass);

        /// <summary>Tonne (metric ton) — 1,000,000 grams.</summary>
        public static readonly Unit Tonne = new("t", 1_000_000, Dimension.Mass);

        internal static readonly Unit[] All =
        [
            Milligram, Gram, Kilogram, Tonne,
        ];
    }
}

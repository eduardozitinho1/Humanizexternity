namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Area units, from square millimeters up to square kilometers and hectares.
    /// </summary>
    /// <remarks>
    /// The base unit is the square meter. Land measurements commonly use hectares
    /// (1 ha = 10,000 m²), which is included alongside the SI squares for practical use.
    /// Symbols use the ASCII digit 2 ("m2") for compatibility with the parser; users can
    /// also type "squaremeter" as an alias.
    /// </remarks>
    public static class Area
    {
        /// <summary>Square millimeter — one millionth of a square meter.</summary>
        public static readonly Unit SquareMillimeter = new("mm2", 0.000001, Dimension.Area);

        /// <summary>Square centimeter — one ten-thousandth of a square meter.</summary>
        public static readonly Unit SquareCentimeter = new("cm2", 0.0001, Dimension.Area);

        /// <summary>Square meter — the base unit of area.</summary>
        public static readonly Unit SquareMeter = new("m2", 1, Dimension.Area);

        /// <summary>Hectare — 10,000 square meters. Common for land measurement.</summary>
        public static readonly Unit Hectare = new("ha", 10_000, Dimension.Area);

        /// <summary>Square kilometer — one million square meters.</summary>
        public static readonly Unit SquareKilometer = new("km2", 1_000_000, Dimension.Area);

        internal static readonly Unit[] All =
        [
            SquareMillimeter,
            SquareCentimeter,
            SquareMeter,
            Hectare,
            SquareKilometer,
        ];
    }
}

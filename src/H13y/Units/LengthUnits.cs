namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Length units, from millimeters up to kilometers.
    /// </summary>
    /// <remarks>
    /// The base unit is the meter. Factors are powers of ten, following the SI system.
    /// Imperial units (inch, foot, mile) are intentionally not included in v0.1.0 to keep
    /// the API focused; they may be added in a future version if there is demand.
    /// </remarks>
    public static class Length
    {
        /// <summary>Millimeter — one thousandth of a meter.</summary>
        public static readonly Unit Millimeter = new("mm", 0.001, Dimension.Length);

        /// <summary>Centimeter — one hundredth of a meter.</summary>
        public static readonly Unit Centimeter = new("cm", 0.01, Dimension.Length);

        /// <summary>Meter — the base unit of length.</summary>
        public static readonly Unit Meter = new("m", 1, Dimension.Length);

        /// <summary>Kilometer — 1000 meters.</summary>
        public static readonly Unit Kilometer = new("km", 1000, Dimension.Length);

        internal static readonly Unit[] All =
        [
            Millimeter, Centimeter, Meter, Kilometer,
        ];
    }
}

namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Volume units, from milliliters up to cubic meters.
    /// </summary>
    /// <remarks>
    /// The base unit is the liter, which keeps everyday quantities (drinks, containers)
    /// intuitive while still supporting scientific small values through milliliters and
    /// large industrial volumes through cubic meters. All factors are powers of ten.
    ///
    /// Note that centiliter is registered as a public <see cref="Unit"/> and recognized by
    /// the parser, but it is intentionally excluded from <see cref="All"/> so that
    /// <see cref="BestUnitSelector"/> never picks it for automatic display. In everyday
    /// language, "500 ml" is far more natural than "50 cl", even though both are correct.
    /// Users who explicitly want centiliters can still call <c>Volume.FromCentiliters</c>
    /// or parse a string containing "cl".
    /// </remarks>
    public static class Volume
    {
        /// <summary>Milliliter — one thousandth of a liter.</summary>
        public static readonly Unit Milliliter = new("ml", 0.001, Dimension.Volume);

        /// <summary>Centiliter — one hundredth of a liter. Recognized by the parser but not auto-selected.</summary>
        public static readonly Unit Centiliter = new("cl", 0.01, Dimension.Volume);

        /// <summary>Liter — the base unit of volume.</summary>
        public static readonly Unit Liter = new("l", 1, Dimension.Volume);

        /// <summary>Cubic meter — 1000 liters.</summary>
        public static readonly Unit CubicMeter = new("m3", 1000, Dimension.Volume);

        internal static readonly Unit[] All = [Milliliter, Liter, CubicMeter];
    }
}

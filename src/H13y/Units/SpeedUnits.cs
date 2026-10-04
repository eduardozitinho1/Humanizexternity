namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Speed units. Base: meter per second (m/s).
    /// </summary>
    /// <remarks>
    /// Auto unit selection only considers km/h so everyday speeds read consistently.
    /// Meter per second, mile per hour, knot, and foot per second remain available
    /// through the parser, the typed <c>Speed</c> record, and <see cref="H.Convert"/>.
    /// </remarks>
    public static class Speed
    {
        /// <summary>Meter per second — the base unit of speed.</summary>
        public static readonly Unit MeterPerSecond = new("m/s", 1, Dimension.Speed);

        /// <summary>Kilometer per hour — the default auto-selected unit.</summary>
        public static readonly Unit KilometerPerHour = new("km/h", 1000.0 / 3600, Dimension.Speed);

        /// <summary>Mile per hour.</summary>
        public static readonly Unit MilePerHour = new("mph", 1609.344 / 3600, Dimension.Speed);

        /// <summary>Knot — one nautical mile per hour.</summary>
        public static readonly Unit Knot = new("kn", 1852.0 / 3600, Dimension.Speed);

        /// <summary>Foot per second.</summary>
        public static readonly Unit FootPerSecond = new("ft/s", 0.3048, Dimension.Speed);

        internal static readonly Unit[] All = [ KilometerPerHour ];
    }
}

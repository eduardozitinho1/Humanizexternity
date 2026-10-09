namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Pressure units. Base: pascal (Pa).
    /// </summary>
    public static class Pressure
    {
        /// <summary>Pascal — the base unit of pressure.</summary>
        public static readonly Unit Pascal = new("Pa", 1, Dimension.Pressure);

        /// <summary>Kilopascal — 1000 pascals.</summary>
        public static readonly Unit Kilopascal = new("kPa", 1000, Dimension.Pressure);

        /// <summary>Megapascal — 1,000,000 pascals.</summary>
        public static readonly Unit Megapascal = new("MPa", 1_000_000, Dimension.Pressure);

        /// <summary>Bar — exactly 100,000 pascals.</summary>
        public static readonly Unit Bar = new("bar", 100_000, Dimension.Pressure);

        /// <summary>Pound per square inch — 6894.757 pascals.</summary>
        public static readonly Unit Psi = new("psi", 6894.757, Dimension.Pressure);

        /// <summary>Standard atmosphere — 101,325 pascals.</summary>
        public static readonly Unit Atmosphere = new("atm", 101_325, Dimension.Pressure);

        internal static readonly Unit[] All = [Pascal, Kilopascal, Megapascal, Bar];
    }
}

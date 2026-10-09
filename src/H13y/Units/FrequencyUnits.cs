namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Frequency units. Base: hertz (Hz).
    /// </summary>
    public static class Frequency
    {
        /// <summary>Hertz — one cycle per second.</summary>
        public static readonly Unit Hertz = new("Hz", 1, Dimension.Frequency);

        /// <summary>Kilohertz — 1000 hertz.</summary>
        public static readonly Unit Kilohertz = new("kHz", 1000, Dimension.Frequency);

        /// <summary>Megahertz — 1,000,000 hertz.</summary>
        public static readonly Unit Megahertz = new("MHz", 1_000_000, Dimension.Frequency);

        /// <summary>Gigahertz — 1,000,000,000 hertz.</summary>
        public static readonly Unit Gigahertz = new("GHz", 1_000_000_000, Dimension.Frequency);

        internal static readonly Unit[] All = [Hertz, Kilohertz, Megahertz, Gigahertz];
    }
}

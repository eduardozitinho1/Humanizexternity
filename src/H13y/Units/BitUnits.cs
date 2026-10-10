namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Digital information measured in bits, from bits up to terabits.
    /// </summary>
    /// <remarks>
    /// Bits are used for network bandwidth and transfer rates. Unlike <see cref="Data"/>
    /// (bytes), which follows the 1024-based convention, bit units here use 1000-based
    /// scaling because that is the SI standard and what telecommunications and ISPs use.
    /// IEC binary symbols (Kib, Mib, ...) are not registered in this dimension to avoid
    /// conflicting with the byte dimension, which owns them.
    /// </remarks>
    public static class Bits
    {
        /// <summary>Bit — the base unit of digital information in this dimension.</summary>
        public static readonly Unit Bit = new("b", 1, Dimension.Bits);

        /// <summary>Kilobit — 1000 bits.</summary>
        public static readonly Unit Kilobit = new("Kb", 1_000, Dimension.Bits);

        /// <summary>Megabit — 1,000,000 bits.</summary>
        public static readonly Unit Megabit = new("Mb", 1_000_000, Dimension.Bits);

        /// <summary>Gigabit — 1,000,000,000 bits.</summary>
        public static readonly Unit Gigabit = new("Gb", 1_000_000_000, Dimension.Bits);

        /// <summary>Terabit — 1,000,000,000,000 bits.</summary>
        public static readonly Unit Terabit = new("Tb", 1_000_000_000_000, Dimension.Bits);

        /// <summary>Kibibit — 1024 bits. IEC binary unit.</summary>
        public static readonly Unit Kibibit = new("Kib", 1_024, Dimension.Bits);

        /// <summary>Mebibit — 1,048,576 bits. IEC binary unit.</summary>
        public static readonly Unit Mebibit = new("Mib", 1_048_576, Dimension.Bits);

        /// <summary>Gibibit — 1,073,741,824 bits. IEC binary unit.</summary>
        public static readonly Unit Gibibit = new("Gib", 1_073_741_824, Dimension.Bits);

        /// <summary>Tebibit — 1,099,511,627,776 bits. IEC binary unit.</summary>
        public static readonly Unit Tebibit = new("Tib", 1_099_511_627_776, Dimension.Bits);

        internal static readonly Unit[] All = [Bit, Kilobit, Megabit, Gigabit, Terabit];
    }
}

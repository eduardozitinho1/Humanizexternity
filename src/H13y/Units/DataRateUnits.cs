namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Data transfer rate units. Base: bit per second (bps).
    /// </summary>
    /// <remarks>
    /// Two sub-families coexist here: bit-based rates (bps, Kbps, Mbps, Gbps, Tbps),
    /// which are SI 1000-based and used by ISPs and network equipment, and byte-based
    /// rates (B/s, KB/s, MB/s, GB/s, TB/s), where the factor is 8x the corresponding
    /// bit rate because there are eight bits in a byte. Only the bit-based chain is
    /// auto-selected; byte-based rates are available through parsing and conversion.
    /// </remarks>
    public static class DataRate
    {
        /// <summary>Bit per second — the base unit.</summary>
        public static readonly Unit BitPerSecond = new("bps", 1, Dimension.DataRate);

        /// <summary>Kilobit per second — 1000 bits per second.</summary>
        public static readonly Unit KilobitPerSecond = new("Kbps", 1_000, Dimension.DataRate);

        /// <summary>Megabit per second — 1,000,000 bits per second.</summary>
        public static readonly Unit MegabitPerSecond = new("Mbps", 1_000_000, Dimension.DataRate);

        /// <summary>Gigabit per second — 1,000,000,000 bits per second.</summary>
        public static readonly Unit GigabitPerSecond = new(
            "Gbps",
            1_000_000_000,
            Dimension.DataRate
        );

        /// <summary>Terabit per second — 1,000,000,000,000 bits per second.</summary>
        public static readonly Unit TerabitPerSecond = new(
            "Tbps",
            1_000_000_000_000,
            Dimension.DataRate
        );

        /// <summary>Byte per second — 8 bits per second.</summary>
        public static readonly Unit BytePerSecond = new("B/s", 8, Dimension.DataRate);

        /// <summary>Kilobyte per second — 8000 bits per second.</summary>
        public static readonly Unit KilobytePerSecond = new("KB/s", 8_000, Dimension.DataRate);

        /// <summary>Megabyte per second — 8,000,000 bits per second.</summary>
        public static readonly Unit MegabytePerSecond = new(
            "MB/s",
            8_000_000,
            Dimension.DataRate
        );

        /// <summary>Gigabyte per second — 8,000,000,000 bits per second.</summary>
        public static readonly Unit GigabytePerSecond = new(
            "GB/s",
            8_000_000_000,
            Dimension.DataRate
        );

        /// <summary>Terabyte per second — 8,000,000,000,000 bits per second.</summary>
        public static readonly Unit TerabytePerSecond = new(
            "TB/s",
            8_000_000_000_000,
            Dimension.DataRate
        );

        internal static readonly Unit[] All =
        [
            BitPerSecond,
            KilobitPerSecond,
            MegabitPerSecond,
            GigabitPerSecond,
            TerabitPerSecond,
        ];
    }
}

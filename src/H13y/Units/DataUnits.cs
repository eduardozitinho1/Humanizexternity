namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Digital information units, from bytes up to petabytes.
    /// </summary>
    /// <remarks>
    /// All factors are 1024-based. Each unit has both a traditional symbol (KB, MB, ...)
    /// and an IEC symbol (KiB, MiB, ...). The displayed symbol is controlled by
    /// <see cref="HumanizeOptions.UseIecSymbols"/>; the numeric factor never changes.
    /// </remarks>
    public static class Data
    {
        /// <summary>Byte — the base unit of digital information.</summary>
        public static readonly Unit Byte = new("B", 1, Dimension.Data);

        /// <summary>Kilobyte — 1024 bytes. IEC symbol: KiB.</summary>
        public static readonly Unit Kilobyte = new("KB", 1024, Dimension.Data, 0, "KiB");

        /// <summary>Megabyte — 1024 kilobytes. IEC symbol: MiB.</summary>
        public static readonly Unit Megabyte = new("MB", 1024.0 * 1024, Dimension.Data, 0, "MiB");

        /// <summary>Gigabyte — 1024 megabytes. IEC symbol: GiB.</summary>
        public static readonly Unit Gigabyte = new(
            "GB",
            1024.0 * 1024 * 1024,
            Dimension.Data,
            0,
            "GiB"
        );

        /// <summary>Terabyte — 1024 gigabytes. IEC symbol: TiB.</summary>
        public static readonly Unit Terabyte = new(
            "TB",
            1024.0 * 1024 * 1024 * 1024,
            Dimension.Data,
            0,
            "TiB"
        );

        /// <summary>Petabyte — 1024 terabytes. IEC symbol: PiB.</summary>
        public static readonly Unit Petabyte = new(
            "PB",
            1024.0 * 1024 * 1024 * 1024 * 1024,
            Dimension.Data,
            0,
            "PiB"
        );

        internal static readonly Unit[] All =
        [
            Byte,
            Kilobyte,
            Megabyte,
            Gigabyte,
            Terabyte,
            Petabyte,
        ];
    }
}

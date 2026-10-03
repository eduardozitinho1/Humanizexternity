namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Digital information units, from bytes up to petabytes.
    /// </summary>
    /// <remarks>
    /// All factors are 1024-based, which is the convention used by operating systems,
    /// RAM, and most developer tooling. The base unit is the byte (<see cref="Byte"/>).
    ///
    /// If you need SI-compatible 1000-based scaling, set <see cref="HumanizeOptions.Mode"/>
    /// to <see cref="DecimalMode.Metric"/> when formatting; the unit symbols stay the same
    /// but the divisor changes.
    /// </remarks>
    public static class Data
    {
        /// <summary>Byte — the base unit of digital information.</summary>
        public static readonly Unit Byte = new("B", 1, Dimension.Data);

        /// <summary>Kilobyte — 1024 bytes.</summary>
        public static readonly Unit Kilobyte = new("KB", 1024, Dimension.Data);

        /// <summary>Megabyte — 1024 kilobytes.</summary>
        public static readonly Unit Megabyte = new("MB", 1024.0 * 1024, Dimension.Data);

        /// <summary>Gigabyte — 1024 megabytes.</summary>
        public static readonly Unit Gigabyte = new("GB", 1024.0 * 1024 * 1024, Dimension.Data);

        /// <summary>Terabyte — 1024 gigabytes.</summary>
        public static readonly Unit Terabyte = new("TB", 1024.0 * 1024 * 1024 * 1024, Dimension.Data);

        /// <summary>Petabyte — 1024 terabytes.</summary>
        public static readonly Unit Petabyte = new("PB", 1024.0 * 1024 * 1024 * 1024 * 1024, Dimension.Data);

        internal static readonly Unit[] All =
        [
            Byte, Kilobyte, Megabyte, Gigabyte, Terabyte, Petabyte,
        ];
    }
}

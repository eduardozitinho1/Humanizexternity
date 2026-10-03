namespace H13y;

public static partial class Units
{
    /// <summary>Digital information units (1024-based).</summary>
    public static class Data
    {
        public static readonly Unit Byte = new("B", 1, Dimension.Data);
        public static readonly Unit Kilobyte = new("KB", 1024, Dimension.Data);
        public static readonly Unit Megabyte = new("MB", 1024.0 * 1024, Dimension.Data);
        public static readonly Unit Gigabyte = new("GB", 1024.0 * 1024 * 1024, Dimension.Data);
        public static readonly Unit Terabyte = new("TB", 1024.0 * 1024 * 1024 * 1024, Dimension.Data);
        public static readonly Unit Petabyte = new("PB", 1024.0 * 1024 * 1024 * 1024 * 1024, Dimension.Data);

        internal static readonly Unit[] All =
        [
            Byte, Kilobyte, Megabyte, Gigabyte, Terabyte, Petabyte,
        ];
    }
}

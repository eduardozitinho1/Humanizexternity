namespace H13y;

public static partial class Units
{
    /// <summary>Time units. Base: second.</summary>
    public static class Time
    {
        public static readonly Unit Millisecond = new("ms", 0.001, Dimension.Time);
        public static readonly Unit Second = new("s", 1, Dimension.Time);
        public static readonly Unit Minute = new("min", 60, Dimension.Time);
        public static readonly Unit Hour = new("h", 3600, Dimension.Time);
        public static readonly Unit Day = new("d", 86400, Dimension.Time);
        public static readonly Unit Week = new("w", 604800, Dimension.Time);

        internal static readonly Unit[] All =
        [
            Millisecond, Second, Minute, Hour, Day, Week,
        ];
    }
}

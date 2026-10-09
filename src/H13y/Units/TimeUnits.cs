namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Time units, from milliseconds up to weeks.
    /// </summary>
    /// <remarks>
    /// The base unit is the second. Unlike mass and length, time does not scale by powers
    /// of ten — minutes are 60 seconds, hours are 3600, days are 86400, weeks are 604800.
    /// Because of that, <see cref="HumanizeFormatter.FormatDuration"/> uses compound
    /// notation ("1 h 30 min 5 s") instead of picking a single unit like the other dimensions do.
    /// </remarks>
    public static class Time
    {
        /// <summary>Millisecond — one thousandth of a second.</summary>
        public static readonly Unit Millisecond = new("ms", 0.001, Dimension.Time);

        /// <summary>Second — the base unit of time.</summary>
        public static readonly Unit Second = new("s", 1, Dimension.Time);

        /// <summary>Minute — 60 seconds.</summary>
        public static readonly Unit Minute = new("min", 60, Dimension.Time);

        /// <summary>Hour — 3600 seconds.</summary>
        public static readonly Unit Hour = new("h", 3600, Dimension.Time);

        /// <summary>Day — 86400 seconds.</summary>
        public static readonly Unit Day = new("d", 86400, Dimension.Time);

        /// <summary>Week — 604800 seconds.</summary>
        public static readonly Unit Week = new("w", 604800, Dimension.Time);

        internal static readonly Unit[] All = [Millisecond, Second, Minute, Hour, Day, Week];
    }
}

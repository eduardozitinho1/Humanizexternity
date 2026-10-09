namespace H13y;

public static partial class H
{
    /// <summary>
    /// Formats the difference between <paramref name="target"/> and the current time as a
    /// human-readable phrase such as "5 minutes ago" or "in 2 hours".
    /// </summary>
    /// <param name="target">The moment to describe.</param>
    /// <param name="now">Optional reference time. Defaults to <see cref="DateTime.UtcNow"/>.</param>
    /// <param name="options">
    /// Optional formatting options. <see cref="HumanizeOptions.Culture"/>,
    /// <see cref="HumanizeOptions.UnitStyle"/>, <see cref="HumanizeOptions.Pluralize"/>, and
    /// <see cref="HumanizeOptions.SpaceBetweenValueAndUnit"/> are honored.
    /// When null, full unit names are used ("5 minutes ago").
    /// </param>
    /// <returns>A phrase such as "just now", "5 minutes ago", or "in 2 hours".</returns>
    public static string RelativeTime(
        DateTime target,
        DateTime? now = null,
        HumanizeOptions? options = null
    )
    {
        var reference = now ?? DateTime.UtcNow;
        var opts = options ?? HumanizeOptions.Default with { UnitStyle = UnitStyle.FullName };
        return RelativeTimeCore(target, reference, opts);
    }

    /// <summary>
    /// Formats the difference between <paramref name="target"/> and the current time as a
    /// human-readable phrase.
    /// </summary>
    public static string RelativeTime(
        DateTimeOffset target,
        DateTimeOffset? now = null,
        HumanizeOptions? options = null
    ) => RelativeTime(target.UtcDateTime, (now ?? DateTimeOffset.UtcNow).UtcDateTime, options);

    private static string RelativeTimeCore(DateTime target, DateTime now, HumanizeOptions options)
    {
        var delta = target - now;
        var future = delta.Ticks > 0;
        var abs = delta.Duration();

        if (abs.TotalSeconds < 45)
            return future ? "in a moment" : "just now";

        var (value, symbol) = PickBucket(abs);
        var rounded = Math.Round(value, MidpointRounding.AwayFromZero);

        var number = NumberFormatter.Format(rounded, options.MaxDecimals, options.Culture);
        var label = UnitNames.Resolve(symbol, rounded, options.UnitStyle, options.Pluralize);
        var core = options.SpaceBetweenValueAndUnit ? $"{number} {label}" : $"{number}{label}";

        return future ? $"in {core}" : $"{core} ago";
    }

    private static (double Value, string Symbol) PickBucket(TimeSpan span)
    {
        if (span.TotalSeconds < 60)
            return (span.TotalSeconds, "s");
        if (span.TotalMinutes < 60)
            return (span.TotalMinutes, "min");
        if (span.TotalHours < 24)
            return (span.TotalHours, "h");
        if (span.TotalDays < 7)
            return (span.TotalDays, "d");
        if (span.TotalDays < 30)
            return (span.TotalDays / 7.0, "w");
        if (span.TotalDays < 365)
            return (span.TotalDays / 30.44, "mo");
        return (span.TotalDays / 365.25, "y");
    }
}

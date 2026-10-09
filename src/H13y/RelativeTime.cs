using Microsoft.Extensions.Localization;
using H13y.Localization;

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
    /// Optional formatting options. Culture, UnitStyle, Pluralize, SpaceBetweenValueAndUnit,
    /// and Localizer are honored. When null, full unit names are used ("5 minutes ago").
    /// </param>
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
            return Localize(
                options.Localizer,
                future ? LocalizationKeys.RelativeTimeMoment : LocalizationKeys.RelativeTimeNow,
                future ? "in a moment" : "just now"
            );

        var (value, symbol) = PickBucket(abs);
        var rounded = Math.Round(value, MidpointRounding.AwayFromZero);

        var number = NumberFormatter.Format(rounded, options.MaxDecimals, options.Culture);
        var label = UnitNames.Resolve(
            symbol,
            rounded,
            options.UnitStyle,
            options.Pluralize,
            options.Localizer
        );
        var core = options.SpaceBetweenValueAndUnit ? $"{number} {label}" : $"{number}{label}";

        var template = Localize(
            options.Localizer,
            future ? LocalizationKeys.RelativeTimeIn : LocalizationKeys.RelativeTimeAgo,
            future ? "in {0}" : "{0} ago"
        );

        return string.Format(options.Culture, template, core);
    }

    private static string Localize(IStringLocalizer? localizer, string key, string fallback)
    {
        if (localizer is null)
            return fallback;

        var localized = localizer[key];
        return localized.ResourceNotFound ? fallback : localized.Value;
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

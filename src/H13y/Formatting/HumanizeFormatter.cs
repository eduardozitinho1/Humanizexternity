namespace H13y;

/// <summary>
/// Formats values into human-readable strings.
/// </summary>
public static class HumanizeFormatter
{
    /// <summary>
    /// Formats a value (already in the dimension's base unit) into a human-readable string.
    /// </summary>
    public static string Format(double baseValue, Dimension dimension, HumanizeOptions? options = null)
    {
        ThrowHelper.ThrowIfNegative(baseValue, nameof(baseValue));

        var opts = options ?? HumanizeOptions.Default;
        var unit = BestUnitSelector.Pick(baseValue, dimension);
        var value = baseValue / unit.Factor;

        return Combine(value, unit.Symbol, opts);
    }

    /// <summary>
    /// Formats a duration in seconds using compound notation (1 h 30 min 5 s).
    /// </summary>
    public static string FormatDuration(double seconds, HumanizeOptions? options = null)
    {
        var opts = options ?? HumanizeOptions.Default;

        if (seconds < 1)
            return Combine(seconds * 1000, "ms", opts);

        if (seconds < 60)
            return Combine(seconds, "s", opts);

        if (seconds < 3600)
        {
            int minutes = (int)(seconds / 60);
            double secondsLeft = seconds % 60;
            return secondsLeft > 0
                ? $"{Combine(minutes, "min", opts)} {Combine(secondsLeft, "s", opts)}"
                : Combine(minutes, "min", opts);
        }

        if (seconds < 86400)
        {
            int hours = (int)(seconds / 3600);
            int minutes = (int)((seconds % 3600) / 60);
            double secondsLeft = seconds % 60;

            var parts = new List<string> { Combine(hours, "h", opts) };
            if (minutes > 0) parts.Add(Combine(minutes, "min", opts));
            if (secondsLeft > 0) parts.Add(Combine(secondsLeft, "s", opts));
            return string.Join(" ", parts);
        }

        if (seconds < 604800)
        {
            int days = (int)(seconds / 86400);
            int hours = (int)((seconds % 86400) / 3600);
            return hours > 0
                ? $"{Combine(days, "d", opts)} {Combine(hours, "h", opts)}"
                : Combine(days, "d", opts);
        }

        int weeks = (int)(seconds / 604800);
        int daysLeft = (int)((seconds % 604800) / 86400);
        return daysLeft > 0
            ? $"{Combine(weeks, "w", opts)} {Combine(daysLeft, "d", opts)}"
            : Combine(weeks, "w", opts);
    }

    private static string Combine(double value, string symbol, HumanizeOptions options)
    {
        var number = NumberFormatter.Format(value, options.MaxDecimals, options.Culture);
        return options.SpaceBetweenValueAndUnit
            ? $"{number} {symbol}"
            : $"{number}{symbol}";
    }
}

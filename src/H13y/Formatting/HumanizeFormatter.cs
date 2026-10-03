namespace H13y;

/// <summary>
/// Low-level formatting engine that turns numeric values into human-readable strings.
/// </summary>
/// <remarks>
/// Two formatting strategies are provided:
///
/// <list type="bullet">
///   <item><description><see cref="Format"/> — for scalar dimensions (data, mass, length, time, volume, area). Picks a single best unit and prints the value with that unit.</description></item>
///   <item><description><see cref="FormatDuration"/> — for time, using compound notation because time units are not powers of ten.</description></item>
/// </list>
///
/// Temperature is handled separately by <see cref="TemperatureFormatter"/> because its units
/// are affine, not linear. Passing <see cref="Dimension.Temperature"/> to <see cref="Format"/>
/// throws with a clear message.
///
/// Non-finite inputs are handled explicitly: <see cref="double.NaN"/> returns "NaN",
/// positive infinity returns "∞", and negative infinity returns "-∞".
/// </remarks>
public static class HumanizeFormatter
{
    /// <summary>
    /// Formats a value (already in the dimension's base unit) into a human-readable string.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="baseValue"/> is negative and finite.</exception>
    /// <exception cref="ArgumentException">Thrown when the dimension has no registered units or is <see cref="Dimension.Temperature"/>.</exception>
    public static string Format(double baseValue, Dimension dimension, HumanizeOptions? options = null)
    {
        if (double.IsNaN(baseValue)) return "NaN";
        if (double.IsPositiveInfinity(baseValue)) return "∞";
        if (double.IsNegativeInfinity(baseValue)) return "-∞";

        ThrowHelper.ThrowIfNegative(baseValue, nameof(baseValue));

        var opts = options ?? HumanizeOptions.Default;
        var unit = BestUnitSelector.Pick(baseValue, dimension);
        var value = baseValue / unit.Factor;

        return Combine(value, unit.Symbol, opts);
    }

    /// <summary>Formats a duration in seconds using compound notation.</summary>
    public static string FormatDuration(double seconds, HumanizeOptions? options = null)
    {
        if (double.IsNaN(seconds)) return "NaN";
        if (double.IsPositiveInfinity(seconds)) return "∞";
        if (double.IsNegativeInfinity(seconds)) return "-∞";

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

    /// <summary>
    /// Joins a numeric value and a unit symbol, respecting the spacing option. Shared with
    /// <see cref="TemperatureFormatter"/> so both produce identical formatting.
    /// </summary>
    internal static string Combine(double value, string symbol, HumanizeOptions options)
    {
        var number = NumberFormatter.Format(value, options.MaxDecimals, options.Culture);
        return options.SpaceBetweenValueAndUnit
            ? $"{number} {symbol}"
            : $"{number}{symbol}";
    }
}

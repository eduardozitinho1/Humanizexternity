namespace H13y;

public static class TemperatureDeltaFormatter
{
    public static string Format(
        double kelvinDelta,
        TemperatureScale scale = TemperatureScale.Celsius,
        HumanizeOptions? options = null
    )
    {
        if (double.IsNaN(kelvinDelta))
            return "NaN";
        if (double.IsPositiveInfinity(kelvinDelta))
            return "\u221e";
        if (double.IsNegativeInfinity(kelvinDelta))
            return "-\u221e";

        var opts = options ?? HumanizeOptions.Default;

        var (value, symbol) = scale switch
        {
            TemperatureScale.Celsius => (kelvinDelta, "\u00b0C"),
            TemperatureScale.Fahrenheit => (kelvinDelta * 9.0 / 5.0, "\u00b0F"),
            TemperatureScale.Kelvin => (kelvinDelta, "K"),
            _ => throw new ArgumentOutOfRangeException(
                nameof(scale),
                scale,
                "Unknown temperature scale."
            ),
        };

        var number = NumberFormatter.Format(value, opts.MaxDecimals, opts.Culture);
        var separator = opts.SpaceBetweenValueAndUnit ? " " : "";
        return $"\u0394{number}{separator}{symbol}";
    }
}

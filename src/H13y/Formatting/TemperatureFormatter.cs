namespace H13y;

/// <summary>
/// Formats temperatures, which are affine and therefore cannot use <see cref="BestUnitSelector"/>.
/// </summary>
/// <remarks>
/// Instead of picking a best unit, this formatter takes an explicit <see cref="TemperatureScale"/>.
/// It shares the string-combining logic with <see cref="HumanizeFormatter.Combine"/> so that
/// spacing, decimal places, and culture options behave identically across the library.
///
/// All inputs are expected in kelvin (the base unit of the Temperature dimension). The value
/// is converted to the requested scale, formatted, and combined with the scale symbol ("°C",
/// "°F", or "K").
/// </remarks>
public static class TemperatureFormatter
{
    /// <summary>
    /// Formats a temperature given in kelvin using the specified scale.
    /// </summary>
    /// <param name="kelvin">The temperature in kelvin.</param>
    /// <param name="scale">The scale to display in. Defaults to <see cref="TemperatureScale.Celsius"/>.</param>
    /// <param name="options">Optional formatting options.</param>
    /// <returns>A string such as "25 °C" or "77 °F".</returns>
    public static string Format(
        double kelvin,
        TemperatureScale scale = TemperatureScale.Celsius,
        HumanizeOptions? options = null
    )
    {
        if (double.IsNaN(kelvin))
            return "NaN";
        if (double.IsPositiveInfinity(kelvin))
            return "∞";
        if (double.IsNegativeInfinity(kelvin))
            return "-∞";

        var opts = options ?? HumanizeOptions.Default;

        var (value, symbol) = scale switch
        {
            TemperatureScale.Celsius => (kelvin - 273.15, "°C"),
            TemperatureScale.Fahrenheit => ((kelvin - 273.15) * 9.0 / 5.0 + 32, "°F"),
            TemperatureScale.Kelvin => (kelvin, "K"),
            _ => throw new ArgumentOutOfRangeException(
                nameof(scale),
                scale,
                "Unknown temperature scale."
            ),
        };

        return HumanizeFormatter.Combine(value, symbol, opts);
    }
}

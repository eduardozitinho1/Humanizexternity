using System.Globalization;

namespace H13y;

/// <summary>
/// Formats numeric values for display, trimming unnecessary trailing zeros.
/// </summary>
/// <remarks>
/// This helper exists because the default <see cref="double.ToString(string)"/> format
/// "0.0" would render <c>1.0</c> as "1.0" instead of "1", which looks unpolished in
/// user-facing output. The formatter rounds to the requested number of decimals and
/// then uses a custom format string that omits trailing zeros.
///
/// Rounding uses <see cref="MidpointRounding.AwayFromZero"/>, which matches the way most
/// people expect "round half up" to behave (0.5 rounds to 1 rather than to 0).
/// </remarks>
internal static class NumberFormatter
{
    /// <summary>
    /// Formats a value with up to <paramref name="maxDecimals"/> decimals, omitting trailing zeros.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="maxDecimals">Maximum number of decimal places. Negative values are treated as zero.</param>
    /// <param name="culture">The culture to use for the decimal separator.</param>
    public static string Format(double value, int maxDecimals, CultureInfo culture)
    {
        if (maxDecimals < 0)
            maxDecimals = 0;

        var rounded = Math.Round(value, maxDecimals, MidpointRounding.AwayFromZero);

        var format = maxDecimals > 0
            ? "0." + new string('#', maxDecimals)
            : "0";

        return rounded.ToString(format, culture);
    }
}

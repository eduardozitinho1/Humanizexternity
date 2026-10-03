using System.Globalization;

namespace H13y;

internal static class NumberFormatter
{
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

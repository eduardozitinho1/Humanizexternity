using System.Globalization;

namespace H13y;

public static partial class H
{
    /// <summary>
    /// Formats a decimal number as English words. The integer part is spelled
    /// using <see cref="H.Word(long)"/>; the fractional part spells each digit
    /// individually, preceded by "point". Trailing zeros are trimmed.
    /// </summary>
    /// <remarks>
    /// <c>H.Word(22.5m)</c> becomes <c>"twenty-two point five"</c>.
    /// <c>H.Word(3.14m)</c> becomes <c>"three point one four"</c>.
    /// <c>H.Word(-0.75m)</c> becomes <c>"minus zero point seven five"</c>.
    /// Supports values in <see cref="H.MinWordValue"/>..<see cref="H.MaxWordValue"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is outside the supported range.
    /// </exception>
    public static string Word(decimal value)
    {
        if (value < MinWordValue || value > MaxWordValue)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Word formatting supports values in {MinWordValue}..{MaxWordValue}."
            );

        return WordDecimalFormatter.Format(value);
    }

    /// <summary>
    /// Formats a double as English words, rounding to <paramref name="maxDecimals"/>
    /// decimal places first to remove floating-point artifacts.
    /// </summary>
    /// <remarks>
    /// <c>H.Word(22.5)</c> becomes <c>"twenty-two point five"</c>.
    /// <c>H.Word(0.1 + 0.2)</c> becomes <c>"zero point three"</c> because the
    /// default rounding removes the 4e-17 artifact.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is NaN, infinite, or outside the
    /// decimal range, or when <paramref name="maxDecimals"/> is outside 0..28.
    /// </exception>
    public static string Word(double value, int maxDecimals = 10)
    {
        if (double.IsNaN(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value is NaN.");

        if (double.IsInfinity(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "Value is infinite.");

        if (value > 7.9e28 || value < -7.9e28)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Value is outside the decimal range."
            );

        if (maxDecimals < 0 || maxDecimals > 28)
            throw new ArgumentOutOfRangeException(
                nameof(maxDecimals),
                maxDecimals,
                "maxDecimals must be between 0 and 28."
            );

        var rounded = (decimal)value;
        rounded = Math.Round(rounded, maxDecimals);
        return Word(rounded);
    }

    /// <summary>
    /// Parses English number words that may include a decimal part back to a
    /// decimal value. Accepts the same forms as
    /// <see cref="H.TryParseWord(string, out long)"/> plus the "point"
    /// separator for fractional digits.
    /// </summary>
    /// <remarks>
    /// <c>"twenty-two point five"</c> becomes <c>22.5m</c>.
    /// <c>"three point one four"</c> becomes <c>3.14m</c>.
    /// Each fractional digit must be a single-digit word (zero through nine).
    /// Multi-digit fractional words like "fourteen" are rejected.
    /// </remarks>
    public static bool TryParseDecimalWord(string text, out decimal value) =>
        WordDecimalFormatter.TryParse(text, out value);
}

internal static class WordDecimalFormatter
{
    private const int MaxFractionalDigits = 28;

    public static string Format(decimal value)
    {
        if (value == 0)
            return "zero";

        if (value < 0)
            return "minus " + Format(-value);

        var str = value.ToString(CultureInfo.InvariantCulture);
        var dot = str.IndexOf('.');

        if (dot < 0)
            return WordFormatter.Format((long)value);

        var integer = (long)value;
        var fractional = str[(dot + 1)..].TrimEnd('0');

        if (fractional.Length == 0)
            return WordFormatter.Format(integer);

        var prefix = integer == 0
            ? "zero point "
            : WordFormatter.Format(integer) + " point ";

        var parts = new string[fractional.Length];
        for (var i = 0; i < fractional.Length; i++)
            parts[i] = WordFormatter.Ones[fractional[i] - '0'];

        return prefix + string.Join(' ', parts);
    }

    public static bool TryParse(string text, out decimal value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var normalized = text.Trim().ToLowerInvariant();
        var negative = false;

        if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[6..].Trim();
        }
        else if (normalized.StartsWith("negative ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[9..].Trim();
        }

        if (normalized.Length == 0)
            return false;

        var pointIdx = normalized.IndexOf(" point ", StringComparison.Ordinal);

        if (pointIdx < 0)
        {
            if (!WordFormatter.TryParse(normalized, out var intValue))
                return false;

            value = negative ? -intValue : intValue;
            return true;
        }

        var intPart = normalized[..pointIdx].Trim();
        var fracPart = normalized[(pointIdx + 7)..].Trim();

        if (intPart.Length == 0 || fracPart.Length == 0)
            return false;

        if (!WordFormatter.TryParse(intPart, out var integer))
            return false;

        var fracTokens = fracPart.Split(
            [' ', '-'],
            StringSplitOptions.RemoveEmptyEntries
        );

        if (fracTokens.Length == 0 || fracTokens.Length > MaxFractionalDigits)
            return false;

        decimal fraction = 0m;
        decimal divisor = 10m;

        foreach (var token in fracTokens)
        {
            var digit = -1;
            for (var i = 0; i < 10; i++)
            {
                if (WordFormatter.Ones[i] == token)
                {
                    digit = i;
                    break;
                }
            }

            if (digit < 0)
                return false;

            fraction += digit / divisor;
            divisor *= 10m;
        }

        var result = integer + fraction;
        if (negative)
            result = -result;

        value = result;
        return true;
    }
}

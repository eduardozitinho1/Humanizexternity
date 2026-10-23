namespace H13y;

/// <summary>
/// Spelling of cardinal numbers as English words. Complements
/// <see cref="H.OrdinalWord(long)"/>, which spells ordinals.
/// </summary>
public static partial class H
{
    /// <summary>Largest value accepted by <see cref="H.Word(long)"/>.</summary>
    public const long MaxWordValue = 999_999_999_999_999;

    /// <summary>Smallest value accepted by <see cref="H.Word(long)"/>.</summary>
    public const long MinWordValue = -MaxWordValue;

    /// <summary>
    /// Formats an integer as English words: 22 becomes "twenty-two",
    /// 1234 becomes "one thousand two hundred thirty-four",
    /// -15 becomes "minus fifteen".
    /// </summary>
    /// <remarks>
    /// Supports values in -999,999,999,999,999..999,999,999,999,999.
    /// Zero is rendered as "zero"; negative values are prefixed with
    /// "minus". Compound numbers below 100 hyphenate the tens and ones
    /// ("twenty-two"); hundreds and higher use spaces.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is outside the supported range.
    /// </exception>
    public static string Word(long value) => WordFormatter.Format(value);

    /// <summary>
    /// Parses an English word back to its integer value: "twenty-two" becomes
    /// 22, "one hundred twenty-two" becomes 122, "two million" becomes
    /// 2,000,000. Returns false on malformed input.
    /// </summary>
    /// <remarks>
    /// Accepts tens and ones separated by a hyphen or a space, ignores the
    /// word "and" (as in "one hundred and twenty-two"), and handles
    /// "minus"/"negative" prefixes for negative values. Case-insensitive.
    /// </remarks>
    public static bool TryParseWord(string text, out long value) =>
        WordFormatter.TryParse(text, out value);
}

internal static class WordFormatter
{
    private static readonly string[] Ones =
    [
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
    ];

    private static readonly string[] Teens =
    [
        "ten",
        "eleven",
        "twelve",
        "thirteen",
        "fourteen",
        "fifteen",
        "sixteen",
        "seventeen",
        "eighteen",
        "nineteen",
    ];

    private static readonly string[] Tens =
    [
        "",
        "",
        "twenty",
        "thirty",
        "forty",
        "fifty",
        "sixty",
        "seventy",
        "eighty",
        "ninety",
    ];

    private static readonly (string Name, long Value)[] Multipliers =
    [
        ("trillion", 1_000_000_000_000),
        ("billion", 1_000_000_000),
        ("million", 1_000_000),
        ("thousand", 1_000),
    ];

    public static string Format(long value)
    {
        if (value < H.MinWordValue || value > H.MaxWordValue)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Word formatting supports values in {H.MinWordValue}..{H.MaxWordValue}."
            );

        if (value < 0)
            return "minus " + Format(-value);

        if (value == 0)
            return "zero";

        var parts = new List<string>();

        foreach (var (name, multiplier) in Multipliers)
        {
            if (value >= multiplier)
            {
                long chunk = value / multiplier;
                value %= multiplier;
                parts.Add(FormatBelowThousand(chunk) + " " + name);
            }
        }

        if (value > 0)
            parts.Add(FormatBelowThousand(value));

        return string.Join(" ", parts);
    }

    private static string FormatBelowThousand(long value)
    {
        if (value == 0)
            return "zero";
        if (value < 10)
            return Ones[value];
        if (value < 20)
            return Teens[value - 10];
        if (value < 100)
        {
            long tens = value / 10;
            long ones = value % 10;
            return ones == 0 ? Tens[tens] : $"{Tens[tens]}-{Ones[ones]}";
        }

        long hundreds = value / 100;
        long rest = value % 100;
        string prefix = Ones[hundreds] + " hundred";
        return rest == 0 ? prefix : $"{prefix} {FormatBelowThousand(rest)}";
    }

    public static bool TryParse(string text, out long value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var normalized = text.Trim().ToLowerInvariant();
        bool negative = false;

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

        var tokens = normalized
            .Replace(",", " ")
            .Replace("-", " ")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        long total = 0;
        long current = 0;
        bool sawAny = false;
        bool sawZero = false;
        bool previousWasAnd = false;

        try
        {
            checked
            {
                foreach (var token in tokens)
                {
                    if (token == "and")
                    {
                        if (!sawAny || previousWasAnd)
                            return false;

                        previousWasAnd = true;
                        continue;
                    }

                    previousWasAnd = false;

                    if (token == "zero")
                    {
                        if (sawAny)
                            return false;
                        sawZero = true;
                        sawAny = true;
                        continue;
                    }

                    if (TryParseBelowHundred(token, out var small))
                    {
                        current += small;
                        sawAny = true;
                        continue;
                    }

                    if (token == "hundred")
                    {
                        if (current == 0)
                            return false;
                        current *= 100;
                        continue;
                    }

                    if (TryGetMultiplier(token, out var multiplier))
                    {
                        if (current == 0)
                            current = 1;
                        total += current * multiplier;
                        current = 0;
                        continue;
                    }

                    return false;
                }

                if (previousWasAnd)
                    return false;

                long result = total + current;

                if (sawZero && result != 0)
                    return false;

                if (negative)
                    result = -result;

                if (result < H.MinWordValue || result > H.MaxWordValue)
                    return false;

                value = result;
                return sawAny;
            }
        }
        catch (OverflowException)
        {
            value = 0;
            return false;
        }
    }

    private static bool TryParseBelowHundred(string token, out long value)
    {
        for (int i = 0; i < Ones.Length; i++)
        {
            if (Ones[i] == token)
            {
                value = i;
                return true;
            }
        }

        for (int i = 0; i < Teens.Length; i++)
        {
            if (Teens[i] == token)
            {
                value = 10 + i;
                return true;
            }
        }

        for (int i = 2; i < Tens.Length; i++)
        {
            if (Tens[i] == token)
            {
                value = i * 10;
                return true;
            }
        }

        value = 0;
        return false;
    }

    private static bool TryGetMultiplier(string token, out long value)
    {
        foreach (var (name, multiplier) in Multipliers)
        {
            if (name == token)
            {
                value = multiplier;
                return true;
            }
        }

        value = 0;
        return false;
    }
}

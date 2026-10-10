namespace H13y;

using System.Globalization;

public static partial class H
{
    /// <summary>
    /// Formats an integer as an ordinal with a suffix: 1 becomes "1st",
    /// 22 becomes "22nd", 103 becomes "103rd".
    /// </summary>
    public static string Ordinal(long value, HumanizeOptions? options = null) =>
        OrdinalFormatter.Format(value, options);

    /// <summary>
    /// Formats an integer as an ordinal word: 1 becomes "first",
    /// 21 becomes "twenty-first", 100 becomes "one hundredth".
    /// Supports values in 0..9999.
    /// </summary>
    public static string OrdinalWord(long value) => OrdinalFormatter.Word(value);

    /// <summary>
    /// Parses an ordinal string back to its integer value: "1st" becomes 1,
    /// "22nd" becomes 22, "103rd" becomes 103. Returns false on malformed input
    /// or when the suffix does not match the number.
    /// </summary>
    public static bool TryParseOrdinal(
        string text,
        out long value,
        HumanizeOptions? options = null
    ) => OrdinalFormatter.TryParse(text, out value, options);
}

internal static class OrdinalFormatter
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

    private static readonly string[] OrdinalOnes =
    [
        "zeroth",
        "first",
        "second",
        "third",
        "fourth",
        "fifth",
        "sixth",
        "seventh",
        "eighth",
        "ninth",
    ];

    private static readonly string[] OrdinalTeens =
    [
        "tenth",
        "eleventh",
        "twelfth",
        "thirteenth",
        "fourteenth",
        "fifteenth",
        "sixteenth",
        "seventeenth",
        "eighteenth",
        "nineteenth",
    ];

    private static readonly string[] OrdinalTens =
    [
        "",
        "",
        "twentieth",
        "thirtieth",
        "fortieth",
        "fiftieth",
        "sixtieth",
        "seventieth",
        "eightieth",
        "ninetieth",
    ];

    public static string Format(long value, HumanizeOptions? options = null)
    {
        var opts = options ?? HumanizeOptions.Default;
        return value.ToString(opts.Culture) + SuffixFor(value);
    }

    public static string SuffixFor(long value)
    {
        var abs = Math.Abs(value);
        var mod100 = abs % 100;
        var mod10 = abs % 10;

        return mod100 is >= 11 and <= 13 ? "th"
            : mod10 == 1 ? "st"
            : mod10 == 2 ? "nd"
            : mod10 == 3 ? "rd"
            : "th";
    }

    public static bool TryParse(string text, out long value, HumanizeOptions? options)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var trimmed = text.Trim();
        var opts = options ?? HumanizeOptions.Default;

        string? matched = null;
        foreach (var s in new[] { "st", "nd", "rd", "th" })
        {
            if (trimmed.EndsWith(s, StringComparison.OrdinalIgnoreCase))
            {
                matched = s;
                break;
            }
        }

        if (matched is null)
            return false;

        var numberPart = trimmed[..^matched.Length].Trim();
        if (numberPart.Length == 0)
            return false;

        if (!long.TryParse(numberPart, NumberStyles.Integer, opts.Culture, out var parsed))
            return false;

        var expected = SuffixFor(parsed);
        if (!string.Equals(matched, expected, StringComparison.OrdinalIgnoreCase))
            return false;

        value = parsed;
        return true;
    }

    public static string Word(long value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Ordinal words do not support negative values."
            );

        if (value > 9999)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Ordinal words support values in 0..9999."
            );

        if (value < 1000)
            return WordBelow1000(value);

        var thousands = value / 1000;
        var rest = value % 1000;
        var thousandsWord = Ones[thousands] + " thousand";

        if (rest == 0)
            return thousandsWord + "th";

        return thousandsWord + " " + WordBelow1000(rest);
    }

    private static string WordBelow1000(long value)
    {
        if (value == 0)
            return "zeroth";
        if (value < 10)
            return OrdinalOnes[value];
        if (value < 20)
            return OrdinalTeens[value - 10];
        if (value < 100)
        {
            var tens = value / 10;
            var ones = value % 10;
            return ones == 0 ? OrdinalTens[tens] : $"{Tens[tens]}-{OrdinalOnes[ones]}";
        }

        var hundreds = value / 100;
        var rest = value % 100;
        var prefix = $"{Ones[hundreds]} hundred";
        return rest == 0 ? $"{prefix}th" : $"{prefix} {WordBelow1000(rest)}";
    }
}

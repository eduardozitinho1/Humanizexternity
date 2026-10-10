namespace H13y;

/// <summary>
/// Controls whether compact numbers use short suffixes ("1.5M") or long words ("1.5 million").
/// </summary>
public enum CompactStyle
{
    /// <summary>Short suffixes: K, M, B, T. Produces "1.5M".</summary>
    Suffix = 0,

    /// <summary>Long words: thousand, million, billion, trillion. Produces "1.5 million".</summary>
    Word = 1,
}

public static partial class H
{
    /// <summary>
    /// Formats a raw number in compact notation: 1234 becomes "1.2K",
    /// 1_500_000 becomes "1.5M", 2_300_000_000 becomes "2.3B".
    /// </summary>
    public static string Compact(double value, HumanizeOptions? options = null) =>
        CompactFormatter.Format(value, CompactStyle.Suffix, options);

    /// <summary>
    /// Formats a raw number with long-word suffixes: 1_500_000 becomes "1.5 million".
    /// </summary>
    public static string CompactWords(double value, HumanizeOptions? options = null) =>
        CompactFormatter.Format(value, CompactStyle.Word, options);
}

internal static class CompactFormatter
{
    private static readonly string[] ShortSuffixes = ["", "K", "M", "B", "T"];

    private static readonly string[] LongSuffixes =
    [
        "",
        "thousand",
        "million",
        "billion",
        "trillion",
    ];

    public static string Format(double value, CompactStyle style, HumanizeOptions? options = null)
    {
        if (double.IsNaN(value))
            return "NaN";
        if (double.IsPositiveInfinity(value))
            return "\u221e";
        if (double.IsNegativeInfinity(value))
            return "-\u221e";

        var opts = options ?? HumanizeOptions.Default;
        var abs = Math.Abs(value);
        var sign = value < 0 ? -1 : 1;

        var (scaled, tier) = PickTier(abs);
        var signed = sign * scaled;

        if (tier < ShortSuffixes.Length - 1)
        {
            var rounded = Math.Round(signed, opts.MaxDecimals, MidpointRounding.AwayFromZero);
            if (Math.Abs(rounded) >= 1000)
            {
                tier++;
                signed = sign * (abs / Math.Pow(1000, tier));
            }
        }

        var number = NumberFormatter.Format(signed, opts.MaxDecimals, opts.Culture);
        var suffixes = style == CompactStyle.Suffix ? ShortSuffixes : LongSuffixes;
        var suffix = suffixes[tier];

        if (string.IsNullOrEmpty(suffix))
            return number;

        var separator = style == CompactStyle.Suffix ? "" : " ";
        return $"{number}{separator}{suffix}";
    }

    private static (double Scaled, int Tier) PickTier(double abs)
    {
        if (abs < 1_000)
            return (abs, 0);
        if (abs < 1_000_000)
            return (abs / 1_000, 1);
        if (abs < 1_000_000_000)
            return (abs / 1_000_000, 2);
        if (abs < 1_000_000_000_000)
            return (abs / 1_000_000_000, 3);
        return (abs / 1_000_000_000_000, 4);
    }
}

using System.Text;

namespace H13y;

/// <summary>
/// The style used when emitting Roman numerals.
/// </summary>
public enum RomanStyle
{
    /// <summary>Subtractive (standard): 4 is IV, 9 is IX.</summary>
    Subtractive = 0,

    /// <summary>Additive: 4 is IIII, 9 is VIIII.</summary>
    Additive = 1,
}

public static partial class H
{
    /// <summary>
    /// Converts an integer in 1..3999 to a Roman numeral: 2024 becomes "MMXXIV".
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside 1..3999.</exception>
    public static string Roman(int value, RomanStyle style = RomanStyle.Subtractive) =>
        RomanConverter.ToRoman(value, style);

    /// <summary>
    /// Parses a Roman numeral into its integer value.
    /// </summary>
    /// <exception cref="FormatException">Thrown when the input is empty or not a valid Roman numeral.</exception>
    public static int ParseRoman(string text)
    {
        if (!RomanConverter.TryParse(text, out var value))
            throw new FormatException($"'{text}' is not a valid Roman numeral.");
        return value;
    }

    /// <summary>
    /// Attempts to parse a Roman numeral into its integer value without throwing.
    /// </summary>
    public static bool TryParseRoman(string text, out int value) =>
        RomanConverter.TryParse(text, out value);
}

internal static class RomanConverter
{
    private static readonly (int Value, string Symbol)[] SubtractiveTable =
    [
        (1000, "M"),
        (900, "CM"),
        (500, "D"),
        (400, "CD"),
        (100, "C"),
        (90, "XC"),
        (50, "L"),
        (40, "XL"),
        (10, "X"),
        (9, "IX"),
        (5, "V"),
        (4, "IV"),
        (1, "I"),
    ];

    private static readonly (int Value, string Symbol)[] AdditiveTable =
    [
        (1000, "M"),
        (500, "D"),
        (100, "C"),
        (50, "L"),
        (10, "X"),
        (5, "V"),
        (1, "I"),
    ];

    public static string ToRoman(int value, RomanStyle style)
    {
        if (value < 1 || value > 3999)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Roman numerals support values in 1..3999."
            );

        var table = style == RomanStyle.Subtractive ? SubtractiveTable : AdditiveTable;
        var builder = new StringBuilder();
        var remaining = value;

        foreach (var (v, s) in table)
        {
            while (remaining >= v)
            {
                builder.Append(s);
                remaining -= v;
            }
        }

        return builder.ToString();
    }

    public static bool TryParse(string text, out int value)
    {
        value = 0;

        if (string.IsNullOrEmpty(text))
            return false;

        var upper = text.ToUpperInvariant();
        var total = 0;
        var prev = 0;

        foreach (var c in upper)
        {
            var v = CharValue(c);
            if (v == 0)
                return false;

            total += v > prev ? v - 2 * prev : v;
            prev = v;
        }

        if (total < 1 || total > 3999)
            return false;

        // Round-trip validation rejects non-canonical forms like "IIII" or "IC".
        if (!string.Equals(ToRoman(total, RomanStyle.Subtractive), upper, StringComparison.Ordinal))
            return false;

        value = total;
        return true;
    }

    private static int CharValue(char c) =>
        c switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => 0,
        };
}

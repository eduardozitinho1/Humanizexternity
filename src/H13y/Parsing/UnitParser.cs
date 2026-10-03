using System.Globalization;
using System.Text.RegularExpressions;

namespace H13y;

/// <summary>
/// Parses human-readable strings into <see cref="Measure"/> values.
/// </summary>
public static class UnitParser
{
    private static readonly Regex Pattern = new(
        @"^(?<value>[-+]?\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z]+)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static Measure Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var trimmed = text.Trim();
        if (trimmed.Length == 0)
            throw new FormatException("Input is empty.");

        var match = Pattern.Match(trimmed);
        if (!match.Success)
            throw new FormatException($"Could not parse '{text}'.");

        var valueText = match.Groups["value"].Value.Replace(',', '.');
        if (!double.TryParse(valueText, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            throw new FormatException($"Could not parse number '{valueText}'.");

        var symbol = match.Groups["unit"].Value;
        var unit = UnitAliases.Resolve(symbol)
            ?? throw new FormatException($"Unknown unit '{symbol}'.");

        return new Measure(value, unit);
    }

    public static bool TryParse(string text, out Measure measure)
    {
        try
        {
            measure = Parse(text);
            return true;
        }
        catch (FormatException)
        {
            measure = default;
            return false;
        }
    }
}

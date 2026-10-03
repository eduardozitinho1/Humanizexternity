using System.Globalization;
using System.Text.RegularExpressions;

namespace H13y;

/// <summary>
/// Parses human-readable strings into <see cref="Measure"/> values.
/// </summary>
/// <remarks>
/// The parser accepts strings like "1 GB", "1.5 KB", "500 B", "1,5 kg", "1500 g", "500 ml",
/// and "1 m3". Whitespace between the number and the unit is optional, so "1GB" is accepted.
///
/// Unit symbols may contain digits (for example "m3", "m2", "km2") which is why the
/// unit pattern allows <c>[A-Za-z0-9]+</c>.
///
/// Parsing is intentionally strict: unknown unit symbols throw <see cref="FormatException"/>.
/// Use <see cref="TryParse"/> when you prefer a boolean result over an exception.
/// </remarks>
public static class UnitParser
{
    private static readonly Regex Pattern = new(
        @"^(?<value>[-+]?\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z0-9]+)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Parses a human-readable string such as "1.5 kg" into a <see cref="Measure"/>.
    /// </summary>
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

    /// <summary>
    /// Attempts to parse a human-readable string into a <see cref="Measure"/> without throwing.
    /// </summary>
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

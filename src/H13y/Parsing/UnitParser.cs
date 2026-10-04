using System.Globalization;
using System.Text.RegularExpressions;

namespace H13y;

/// <summary>
/// Parses human-readable strings into <see cref="Measure"/> values.
/// </summary>
/// <remarks>
/// Three input shapes are supported, tried in this order:
///
/// <list type="number">
///   <item><description><b>Compound duration</b> — "1h30min", "1 h 30 min", "1h30m45s".</description></item>
///   <item><description><b>Colon duration</b> — "1:30" (mm:ss), "1:30:45" (hh:mm:ss).</description></item>
///   <item><description><b>Single value + unit</b> — "1 GB", "25 °C", "1 m3", "100 km/h".</description></item>
/// </list>
///
/// The single-value path uses a manual span-based scanner instead of a regex.
/// Compound and colon durations still use compiled regexes because their grammar is
/// regular and the scanner would be significantly larger for no measurable gain.
/// </remarks>
public static class UnitParser
{
    private static readonly Regex CompoundPattern = new(
        @"^\s*(?:(?<value>\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z]+)\s*){2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex ColonPattern = new(
        @"^\s*(?<a>\d+):(?<b>\d{1,2})(?::(?<c>\d{1,2}))?\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Parses a human-readable string into a <see cref="Measure"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the input is empty, malformed, or uses an unknown unit.</exception>
    public static Measure Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var trimmed = text.Trim();
        if (trimmed.Length == 0)
            throw new FormatException("Input is empty.");

        if (TryParseCompoundDuration(trimmed, out var compound))
            return compound;

        if (TryParseColonDuration(trimmed, out var colon))
            return colon;

        if (!TryParseSingle(trimmed, out var value, out var unitSymbol))
            throw new FormatException($"Could not parse '{text}'.");

        var unit = UnitAliases.Resolve(unitSymbol)
            ?? throw new FormatException($"Unknown unit '{unitSymbol}'.");

        return new Measure(value, unit);
    }

    /// <summary>Attempts to parse a human-readable string into a <see cref="Measure"/> without throwing.</summary>
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

    private static bool TryParseSingle(string text, out double value, out string unitSymbol)
    {
        value = 0;
        unitSymbol = string.Empty;

        var span = text.AsSpan().Trim();
        if (span.IsEmpty) return false;

        var i = 0;
        if (i < span.Length && (span[i] == '+' || span[i] == '-')) i++;

        var digitStart = i;
        while (i < span.Length && char.IsDigit(span[i])) i++;
        if (i == digitStart) return false;

        if (i < span.Length && (span[i] == '.' || span[i] == ','))
        {
            i++;
            var fracStart = i;
            while (i < span.Length && char.IsDigit(span[i])) i++;
            if (i == fracStart) return false;
        }

        var numberSpan = span[..i];
        var rest = span[i..].Trim();

        if (numberSpan.IndexOf(',') >= 0)
        {
            Span<char> buf = stackalloc char[numberSpan.Length];
            for (var j = 0; j < numberSpan.Length; j++)
                buf[j] = numberSpan[j] == ',' ? '.' : numberSpan[j];

            if (!double.TryParse(buf, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return false;
        }
        else
        {
            if (!double.TryParse(numberSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return false;
        }

        if (rest.IsEmpty) return false;

        unitSymbol = rest.ToString();
        return true;
    }

    private static bool TryParseCompoundDuration(string text, out Measure measure)
    {
        measure = default;
        var match = CompoundPattern.Match(text);
        if (!match.Success) return false;

        var valueCaptures = match.Groups["value"].Captures;
        var unitCaptures = match.Groups["unit"].Captures;

        if (valueCaptures.Count != unitCaptures.Count || valueCaptures.Count < 2)
            return false;

        double totalSeconds = 0;

        for (int i = 0; i < valueCaptures.Count; i++)
        {
            var valueText = valueCaptures[i].Value.Replace(',', '.');
            if (!double.TryParse(valueText, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                return false;

            var unit = ResolveTimeUnit(unitCaptures[i].Value);
            if (unit is null) return false;

            totalSeconds += value * unit.Factor;
        }

        measure = new Measure(totalSeconds, Units.Time.Second);
        return true;
    }

    private static bool TryParseColonDuration(string text, out Measure measure)
    {
        measure = default;
        var match = ColonPattern.Match(text);
        if (!match.Success) return false;

        if (!double.TryParse(match.Groups["a"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var a))
            return false;
        if (!double.TryParse(match.Groups["b"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var b))
            return false;

        if (match.Groups["c"].Success)
        {
            if (!double.TryParse(match.Groups["c"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var c))
                return false;
            measure = new Measure(a * 3600 + b * 60 + c, Units.Time.Second);
        }
        else
        {
            measure = new Measure(a * 60 + b, Units.Time.Second);
        }

        return true;
    }

    private static Unit? ResolveTimeUnit(string symbol)
    {
        var unit = UnitAliases.Resolve(symbol);
        if (unit is not null && unit.Dimension == Dimension.Time)
            return unit;

        if (string.Equals(symbol, "m", StringComparison.OrdinalIgnoreCase))
            return Units.Time.Minute;

        return null;
    }
}

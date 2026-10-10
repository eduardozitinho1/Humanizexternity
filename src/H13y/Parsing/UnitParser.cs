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
/// The <see cref="CultureInfo"/> argument determines which character is treated as the
/// decimal separator. A dot-culture rejects "1,5 GB" and accepts "1.5 GB"; a comma-culture
/// (pt-BR, de-DE, fr-FR) does the opposite. This removes the ambiguity that existed in
/// earlier versions, where the parser silently accepted both separators regardless of locale.
/// </remarks>
public static class UnitParser
{
    private static readonly Regex SinglePattern = new(
        @"^(?<value>[-+]?\d+(?:[.,]\d+)?(?:[eE][+-]?\d+)?)\s*(?<unit>[°A-Za-z0-9/ _-]+)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    private static readonly Regex CompoundPattern = new(
        @"^\s*(?:(?<value>[-+]?\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z]+)\s*){2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    private static readonly Regex ColonPattern = new(
        @"^\s*(?<a>\d+):(?<b>\d{1,2})(?::(?<c>\d{1,2}))?\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    /// <summary>
    /// Parses a human-readable string into a <see cref="Measure"/> using the given culture.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="culture"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the input is empty, malformed, or uses an unknown unit.</exception>
    public static Measure Parse(string text, CultureInfo culture)
    {
        var result = ParseDetailed(text, culture);
        if (!result.Success)
            throw new FormatException(result.ErrorMessage ?? "Parse failed.");
        return result.Measure;
    }

    /// <summary>
    /// Parses a string and reports a categorized failure instead of throwing.
    /// </summary>
    public static ParseResult ParseDetailed(string text, CultureInfo culture)
    {
        if (text is null)
            return ParseResult.Fail(ParseErrorKind.EmptyInput, "Input is null.");
        if (culture is null)
            return ParseResult.Fail(ParseErrorKind.EmptyInput, "Culture is null.");

        var trimmed = text.Trim();

        if (trimmed.Length == 0)
            return ParseResult.Fail(ParseErrorKind.EmptyInput, "Input is empty.");

        if (TryParseCompoundDuration(trimmed, culture, out var compound))
            return ParseResult.Ok(compound);

        if (TryParseColonDuration(trimmed, out var colon))
            return ParseResult.Ok(colon);

        var match = SinglePattern.Match(trimmed);

        if (!match.Success)
            return ParseResult.Fail(
                ParseErrorKind.InvalidNumber,
                $"Could not parse '{text}'."
            );

        var valueText = match.Groups["value"].Value;

        if (!double.TryParse(valueText, NumberStyles.Float, culture, out var value))
        {
            var kind = valueText.Contains('e', StringComparison.OrdinalIgnoreCase)
                ? ParseErrorKind.Overflow
                : ParseErrorKind.InvalidNumber;
            return ParseResult.Fail(kind, $"Could not parse number '{valueText}'.");
        }

        if (double.IsInfinity(value))
            return ParseResult.Fail(
                ParseErrorKind.Overflow,
                $"Value '{valueText}' overflows the representable range."
            );

        if (double.IsNaN(value))
            return ParseResult.Fail(
                ParseErrorKind.NonFiniteValue,
                $"Value '{valueText}' is not a number."
            );

        var symbol = match.Groups["unit"].Value;
        var unit = UnitAliases.Resolve(symbol);

        if (unit is null)
            return ParseResult.Fail(
                ParseErrorKind.UnknownUnit,
                $"Unknown unit '{symbol}'."
            );

        return ParseResult.Ok(new Measure(value, unit));
    }

    /// <summary>
    /// Attempts to parse a human-readable string into a <see cref="Measure"/> without throwing.
    /// </summary>
    public static bool TryParse(string text, CultureInfo culture, out Measure measure)
    {
        try
        {
            measure = Parse(text, culture);
            return true;
        }
        catch (FormatException)
        {
            measure = default;
            return false;
        }
        catch (ArgumentNullException)
        {
            measure = default;
            return false;
        }
    }

    private static bool TryParseCompoundDuration(
        string text,
        CultureInfo culture,
        out Measure measure
    )
    {
        measure = default;

        var match = CompoundPattern.Match(text);

        if (!match.Success)
            return false;

        var valueCaptures = match.Groups["value"].Captures;
        var unitCaptures = match.Groups["unit"].Captures;

        if (valueCaptures.Count != unitCaptures.Count || valueCaptures.Count < 2)
        {
            return false;
        }

        double totalSeconds = 0;

        for (int i = 0; i < valueCaptures.Count; i++)
        {
            var valueText = valueCaptures[i].Value;

            if (!double.TryParse(valueText, NumberStyles.Float, culture, out var value))
            {
                return false;
            }

            var unit = ResolveTimeUnit(unitCaptures[i].Value);

            if (unit is null)
                return false;

            totalSeconds += value * unit.Factor;
        }

        if (!double.IsFinite(totalSeconds))
            return false;

        measure = new Measure(totalSeconds, Units.Time.Second);
        return true;
    }

    private static bool TryParseColonDuration(string text, out Measure measure)
    {
        measure = default;

        var match = ColonPattern.Match(text);

        if (!match.Success)
            return false;

        if (
            !double.TryParse(
                match.Groups["a"].Value,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var a
            )
        )
        {
            return false;
        }

        if (
            !double.TryParse(
                match.Groups["b"].Value,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var b
            )
        )
        {
            return false;
        }

        if (b >= 60)
            return false;

        double totalSeconds;

        if (match.Groups["c"].Success)
        {
            if (
                !double.TryParse(
                    match.Groups["c"].Value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var c
                )
            )
            {
                return false;
            }

            if (c >= 60)
                return false;

            totalSeconds = a * 3600 + b * 60 + c;
        }
        else
        {
            totalSeconds = a * 60 + b;
        }

        if (!double.IsFinite(totalSeconds))
            return false;

        measure = new Measure(totalSeconds, Units.Time.Second);
        return true;
    }

    private static Unit? ResolveTimeUnit(string symbol)
    {
        var unit = UnitAliases.Resolve(symbol);

        if (unit is not null && unit.Dimension == Dimension.Time)
            return unit;

        if (string.Equals(symbol, "m", StringComparison.OrdinalIgnoreCase))
        {
            return Units.Time.Minute;
        }

        return null;
    }
}

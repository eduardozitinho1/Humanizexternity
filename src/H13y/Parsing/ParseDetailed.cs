using System.Globalization;

namespace H13y;

public static partial class H
{
    /// <summary>
    /// Parses a human-readable string and reports a categorized failure instead of
    /// throwing.
    /// </summary>
    public static ParseResult ParseDetailed(string text, CultureInfo culture) =>
        UnitParser.ParseDetailed(text, culture);

    /// <summary>
    /// Parses a human-readable string, requiring the result to belong to
    /// <paramref name="expectedDimension"/>. Reports <see cref="ParseErrorKind.DimensionMismatch"/>
    /// when the parse succeeds but the dimension differs.
    /// </summary>
    public static ParseResult ParseDetailed(
        string text,
        Dimension expectedDimension,
        CultureInfo culture
    )
    {
        var result = UnitParser.ParseDetailed(text, culture);
        if (!result.Success)
            return result;

        if (result.Measure.Unit.Dimension != expectedDimension)
        {
            return ParseResult.Fail(
                ParseErrorKind.DimensionMismatch,
                $"Expected {expectedDimension}, got {result.Measure.Unit.Dimension}."
            );
        }

        return result;
    }
}

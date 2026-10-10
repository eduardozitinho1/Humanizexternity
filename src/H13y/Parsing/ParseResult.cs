namespace H13y;

/// <summary>
/// The category of parse failure reported by <see cref="H.ParseDetailed(string, System.Globalization.CultureInfo)"/>.
/// </summary>
public enum ParseErrorKind
{
    /// <summary>No error.</summary>
    None = 0,

    /// <summary>The input was null, empty, or whitespace.</summary>
    EmptyInput,

    /// <summary>The numeric part was malformed or not parseable.</summary>
    InvalidNumber,

    /// <summary>The unit symbol or alias was not recognized.</summary>
    UnknownUnit,

    /// <summary>The parsed unit belongs to a different dimension than expected.</summary>
    DimensionMismatch,

    /// <summary>The numeric value exceeded the representable range.</summary>
    Overflow,

    /// <summary>The parsed value was NaN or infinite.</summary>
    NonFiniteValue,
}

/// <summary>
/// The result of a detailed parse: either a <see cref="Measure"/> or a categorized
/// error with a human-readable message.
/// </summary>
/// <param name="Success">True when parsing succeeded.</param>
/// <param name="Measure">The parsed measure. Only meaningful when <paramref name="Success"/> is true.</param>
/// <param name="ErrorKind">The error category, or <see cref="ParseErrorKind.None"/> on success.</param>
/// <param name="ErrorMessage">A diagnostic message, or null on success.</param>
public readonly record struct ParseResult(
    bool Success,
    Measure Measure,
    ParseErrorKind ErrorKind,
    string? ErrorMessage
)
{
    /// <summary>Creates a success result.</summary>
    public static ParseResult Ok(Measure measure) =>
        new(true, measure, ParseErrorKind.None, null);

    /// <summary>Creates a failure result.</summary>
    public static ParseResult Fail(ParseErrorKind kind, string message) =>
        new(false, default, kind, message);
}

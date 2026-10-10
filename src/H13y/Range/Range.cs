using H13y.Measures;

namespace H13y;

/// <summary>
/// The separator used between the two ends of a range.
/// </summary>
public enum RangeStyle
{
    /// <summary>En dash: "1–10". The default.</summary>
    EnDash = 0,

    /// <summary>ASCII hyphen: "1-10".</summary>
    Hyphen = 1,

    /// <summary>The word "to": "1 to 10".</summary>
    To = 2,
}

public static partial class H
{
    /// <summary>
    /// Formats a numeric range with raw values: 1, 10 becomes "1–10".
    /// </summary>
    public static string Range(double from, double to, HumanizeOptions? options = null) =>
        Range(from, to, RangeStyle.EnDash, options);

    /// <summary>
    /// Formats a numeric range with raw values and an explicit separator style.
    /// </summary>
    public static string Range(
        double from,
        double to,
        RangeStyle style,
        HumanizeOptions? options = null
    ) => RangeFormatter.Format(from, to, style, options);

    /// <summary>
    /// Formats a numeric range with each endpoint humanized in the given dimension:
    /// 1000, 1500 in Data becomes "1000 B–1.5 KB".
    /// Temperature is not supported because it is affine.
    /// </summary>
    public static string Range(
        double from,
        double to,
        Dimension dimension,
        HumanizeOptions? options = null
    ) => Range(from, to, dimension, RangeStyle.EnDash, options);

    /// <summary>
    /// Formats a numeric range with humanized endpoints and an explicit separator style.
    /// </summary>
    public static string Range(
        double from,
        double to,
        Dimension dimension,
        RangeStyle style,
        HumanizeOptions? options = null
    ) => RangeFormatter.FormatDimension(from, to, dimension, style, options);

    /// <summary>
    /// Formats the duration between two instants using the Duration humanizer:
    /// "3 d", "1 h 30 min". Order-independent.
    /// </summary>
    public static string Between(DateTime from, DateTime to, HumanizeOptions? options = null)
    {
        var span = to - from;
        return Duration.FromTimeSpan(span.Duration()).Humanize(options);
    }

    /// <summary>
    /// Formats the duration between two instants using the Duration humanizer.
    /// </summary>
    public static string Between(
        DateTimeOffset from,
        DateTimeOffset to,
        HumanizeOptions? options = null
    ) => Between(from.UtcDateTime, to.UtcDateTime, options);

    /// <summary>
    /// Formats a value with a symmetric uncertainty in the given dimension:
    /// 1500, 50 in Mass becomes "1.5 kg ± 50 g".
    /// </summary>
    public static string Uncertainty(
        double value,
        double uncertainty,
        Dimension dimension,
        HumanizeOptions? options = null
    ) => RangeFormatter.FormatUncertainty(value, uncertainty, dimension, options);
}

internal static class RangeFormatter
{
    public static string Format(
        double from,
        double to,
        RangeStyle style,
        HumanizeOptions? options
    )
    {
        var opts = options ?? HumanizeOptions.Default;
        var fromText = NumberFormatter.Format(from, opts.MaxDecimals, opts.Culture);
        var toText = NumberFormatter.Format(to, opts.MaxDecimals, opts.Culture);
        return $"{fromText}{Separator(style)}{toText}";
    }

    public static string FormatDimension(
        double from,
        double to,
        Dimension dimension,
        RangeStyle style,
        HumanizeOptions? options
    )
    {
        if (dimension == Dimension.Temperature)
            throw new ArgumentException(
                "Dimension.Temperature is affine. Use explicit scales instead.",
                nameof(dimension)
            );

        var opts = options ?? HumanizeOptions.Default;
        var fromText = H.Best(from, dimension, opts);
        var toText = H.Best(to, dimension, opts);
        return $"{fromText}{Separator(style)}{toText}";
    }

    public static string FormatUncertainty(
        double value,
        double uncertainty,
        Dimension dimension,
        HumanizeOptions? options
    )
    {
        if (dimension == Dimension.Temperature)
            throw new ArgumentException(
                "Dimension.Temperature is affine. Use explicit scales instead.",
                nameof(dimension)
            );

        if (uncertainty < 0)
            throw new ArgumentOutOfRangeException(
                nameof(uncertainty),
                uncertainty,
                "Uncertainty must not be negative."
            );

        var opts = options ?? HumanizeOptions.Default;
        var valueText = H.Best(value, dimension, opts);
        var uncertaintyText = H.Best(uncertainty, dimension, opts);
        return $"{valueText} \u00b1 {uncertaintyText}";
    }

    private static string Separator(RangeStyle style) =>
        style switch
        {
            RangeStyle.EnDash => "\u2013",
            RangeStyle.Hyphen => "-",
            RangeStyle.To => " to ",
            _ => "\u2013",
        };
}

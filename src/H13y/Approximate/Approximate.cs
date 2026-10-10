using System.Globalization;
using H13y.Localization;
using Microsoft.Extensions.Localization;

namespace H13y;

/// <summary>
/// The magnitude to which <see cref="H.Approximate(double, ApproximationPrecision, HumanizeOptions?)"/>
/// rounds a value.
/// </summary>
public enum ApproximationPrecision
{
    Thousandth,
    Hundredth,
    Tenth,
    One,
    Ten,
    Hundred,
    Thousand,
    TenThousand,
    HundredThousand,
    Million,
    Billion,
    Trillion,
}

/// <summary>
/// The adverb that prefixes an approximate value.
/// </summary>
public enum ApproximationStyle
{
    About,
    Roughly,
    Approximately,
}

public static partial class H
{
    /// <summary>
    /// Formats a value as a rough estimate: 1234 with <see cref="ApproximationPrecision.Hundred"/>
    /// becomes "about 1200"; 0.784 with <see cref="ApproximationPrecision.Tenth"/> becomes
    /// "about 0.8"; 1_500_000 with <see cref="ApproximationPrecision.Million"/> becomes
    /// "about 2 million".
    /// </summary>
    public static string Approximate(
        double value,
        ApproximationPrecision precision,
        HumanizeOptions? options = null
    ) => ApproximateFormatter.Format(value, precision, ApproximationStyle.About, options);

    /// <summary>
    /// Formats a value as a rough estimate with a custom adverb style.
    /// </summary>
    public static string Approximate(
        double value,
        ApproximationPrecision precision,
        ApproximationStyle style,
        HumanizeOptions? options = null
    ) => ApproximateFormatter.Format(value, precision, style, options);
}

internal static class ApproximateFormatter
{
    private const double CompactThreshold = 10_000;

    public static string Format(
        double value,
        ApproximationPrecision precision,
        ApproximationStyle style,
        HumanizeOptions? options
    )
    {
        var opts = options ?? HumanizeOptions.Default;
        var prefix = LocalizePrefix(style, opts.Localizer);

        if (double.IsNaN(value))
            return $"{prefix} NaN";
        if (double.IsPositiveInfinity(value))
            return $"{prefix} \u221e";
        if (double.IsNegativeInfinity(value))
            return $"{prefix} -\u221e";

        var scale = ScaleFor(precision);
        var rounded = Math.Round(value / scale, MidpointRounding.AwayFromZero) * scale;

        var numberText =
            Math.Abs(rounded) >= CompactThreshold
                ? FormatCompactLong(rounded, opts.Culture)
                : FormatPlain(rounded, precision, opts.Culture);

        return $"{prefix} {numberText}";
    }

    private static double ScaleFor(ApproximationPrecision precision) =>
        precision switch
        {
            ApproximationPrecision.Thousandth => 0.001,
            ApproximationPrecision.Hundredth => 0.01,
            ApproximationPrecision.Tenth => 0.1,
            ApproximationPrecision.One => 1,
            ApproximationPrecision.Ten => 10,
            ApproximationPrecision.Hundred => 100,
            ApproximationPrecision.Thousand => 1_000,
            ApproximationPrecision.TenThousand => 10_000,
            ApproximationPrecision.HundredThousand => 100_000,
            ApproximationPrecision.Million => 1_000_000,
            ApproximationPrecision.Billion => 1_000_000_000,
            ApproximationPrecision.Trillion => 1_000_000_000_000,
            _ => throw new ArgumentOutOfRangeException(nameof(precision)),
        };

    private static string FormatPlain(
        double value,
        ApproximationPrecision precision,
        CultureInfo culture
    )
    {
        var decimals = precision switch
        {
            ApproximationPrecision.Thousandth => 3,
            ApproximationPrecision.Hundredth => 2,
            ApproximationPrecision.Tenth => 1,
            _ => 0,
        };

        return NumberFormatter.Format(value, decimals, culture);
    }

    private static string FormatCompactLong(double value, CultureInfo culture)
    {
        var abs = Math.Abs(value);
        var sign = value < 0 ? -1 : 1;

        double scaled;
        string suffix;

        if (abs >= 1e12)
        {
            scaled = abs / 1e12;
            suffix = "trillion";
        }
        else if (abs >= 1e9)
        {
            scaled = abs / 1e9;
            suffix = "billion";
        }
        else if (abs >= 1e6)
        {
            scaled = abs / 1e6;
            suffix = "million";
        }
        else
        {
            scaled = abs / 1e3;
            suffix = "thousand";
        }

        scaled *= sign;
        var numberText = NumberFormatter.Format(scaled, 3, culture);
        return $"{numberText} {suffix}";
    }

    private static string LocalizePrefix(ApproximationStyle style, IStringLocalizer? localizer)
    {
        var (key, fallback) = style switch
        {
            ApproximationStyle.About => (LocalizationKeys.ApproximateAbout, "about"),
            ApproximationStyle.Roughly => (LocalizationKeys.ApproximateRoughly, "roughly"),
            ApproximationStyle.Approximately => (
                LocalizationKeys.ApproximateApproximately,
                "approximately"
            ),
            _ => (LocalizationKeys.ApproximateAbout, "about"),
        };

        if (localizer is null)
            return fallback;

        var localized = localizer[key];
        return localized.ResourceNotFound ? fallback : localized.Value;
    }
}

using System.Globalization;

namespace H13y;

/// <summary>
/// Shared formatting logic used by the <see cref="IFormattable"/> implementation of every
/// typed measure. Keeps the format-string contract identical across dimensions.
/// </summary>
/// <remarks>
/// Supported formats:
/// <list type="bullet">
///   <item><description><c>null</c>, <c>""</c>, <c>"H"</c> — humanize with the best unit.</description></item>
///   <item><description><c>"I"</c> — humanize using IEC symbols (KiB, MiB, ...) for data dimensions.</description></item>
///   <item><description><c>"R"</c> — raw value in the dimension's base unit.</description></item>
///   <item><description>Anything else — treated as a numeric format string and applied to the value in the best unit, without the unit symbol.</description></item>
/// </list>
/// Temperature also accepts <c>"H:C"</c>, <c>"H:F"</c>, and <c>"H:K"</c> to select the scale explicitly.
/// </remarks>
internal static class FormattableHelpers
{
    public static string FormatScalar(
        double baseValue,
        Dimension dimension,
        string? format,
        IFormatProvider? provider
    )
    {
        var culture = provider as CultureInfo ?? CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(format) || format.Equals("H", StringComparison.OrdinalIgnoreCase))
        {
            var opts = HumanizeOptions.Default with { Culture = culture };
            return HumanizeFormatter.Format(baseValue, dimension, opts);
        }

        if (format.Equals("I", StringComparison.OrdinalIgnoreCase))
        {
            var opts = HumanizeOptions.Default with { Culture = culture, UseIecSymbols = true };
            return HumanizeFormatter.Format(baseValue, dimension, opts);
        }

        if (format.Equals("R", StringComparison.OrdinalIgnoreCase))
            return baseValue.ToString(culture);

        var unit = BestUnitSelector.Pick(baseValue, dimension);
        var value = baseValue / unit.Factor;
        return value.ToString(format, culture);
    }

    public static string FormatDuration(double seconds, string? format, IFormatProvider? provider)
    {
        var culture = provider as CultureInfo ?? CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(format) || format.Equals("H", StringComparison.OrdinalIgnoreCase))
        {
            var opts = HumanizeOptions.Default with { Culture = culture };
            return HumanizeFormatter.FormatDuration(seconds, opts);
        }

        if (format.Equals("R", StringComparison.OrdinalIgnoreCase))
            return seconds.ToString(culture);

        return seconds.ToString(format, culture);
    }

    public static string FormatTemperature(double kelvin, string? format, IFormatProvider? provider)
    {
        var culture = provider as CultureInfo ?? CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(format) || format.Equals("H", StringComparison.OrdinalIgnoreCase))
        {
            var opts = HumanizeOptions.Default with { Culture = culture };
            return TemperatureFormatter.Format(kelvin, TemperatureScale.Celsius, opts);
        }

        if (format.StartsWith("H:", StringComparison.OrdinalIgnoreCase))
        {
            var scale = format[2..].ToUpperInvariant() switch
            {
                "C" => TemperatureScale.Celsius,
                "F" => TemperatureScale.Fahrenheit,
                "K" => TemperatureScale.Kelvin,
                _ => throw new FormatException(
                    $"Unknown temperature scale in format string: '{format}'"
                ),
            };
            var opts = HumanizeOptions.Default with { Culture = culture };
            return TemperatureFormatter.Format(kelvin, scale, opts);
        }

        if (format.Equals("R", StringComparison.OrdinalIgnoreCase))
            return kelvin.ToString(culture);

        var celsius = kelvin - 273.15;
        return celsius.ToString(format, culture);
    }
}

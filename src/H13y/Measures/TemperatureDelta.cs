using System.Globalization;

namespace H13y.Measures;

public readonly record struct TemperatureDelta(double KelvinDelta)
    : IComparable<TemperatureDelta>,
        IFormattable
{
    public static TemperatureDelta FromCelsius(double deltaCelsius) => new(deltaCelsius);

    public static TemperatureDelta FromFahrenheit(double deltaFahrenheit) =>
        new(deltaFahrenheit * 5.0 / 9.0);

    public static TemperatureDelta FromKelvin(double deltaKelvin) => new(deltaKelvin);

    public double ToCelsius() => KelvinDelta;

    public double ToFahrenheit() => KelvinDelta * 9.0 / 5.0;

    public double ToKelvin() => KelvinDelta;

    public static TemperatureDelta operator +(TemperatureDelta left, TemperatureDelta right) =>
        new(left.KelvinDelta + right.KelvinDelta);

    public static TemperatureDelta operator -(TemperatureDelta left, TemperatureDelta right) =>
        new(left.KelvinDelta - right.KelvinDelta);

    public static TemperatureDelta operator -(TemperatureDelta value) => new(-value.KelvinDelta);

    public static bool operator <(TemperatureDelta left, TemperatureDelta right) =>
        left.KelvinDelta < right.KelvinDelta;

    public static bool operator <=(TemperatureDelta left, TemperatureDelta right) =>
        left.KelvinDelta <= right.KelvinDelta;

    public static bool operator >(TemperatureDelta left, TemperatureDelta right) =>
        left.KelvinDelta > right.KelvinDelta;

    public static bool operator >=(TemperatureDelta left, TemperatureDelta right) =>
        left.KelvinDelta >= right.KelvinDelta;

    public static Temperature operator +(Temperature absolute, TemperatureDelta delta) =>
        new(absolute.Kelvin + delta.KelvinDelta);

    public static Temperature operator -(Temperature absolute, TemperatureDelta delta) =>
        new(absolute.Kelvin - delta.KelvinDelta);

    public static TemperatureDelta Difference(Temperature from, Temperature to) =>
        new(to.Kelvin - from.Kelvin);

    public int CompareTo(TemperatureDelta other) => KelvinDelta.CompareTo(other.KelvinDelta);

    public string Humanize(
        TemperatureScale scale = TemperatureScale.Celsius,
        HumanizeOptions? options = null
    ) => TemperatureDeltaFormatter.Format(KelvinDelta, scale, options);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(format) || format.Equals("H", StringComparison.OrdinalIgnoreCase))
        {
            var opts = HumanizeOptions.Default with { Culture = culture };
            return TemperatureDeltaFormatter.Format(KelvinDelta, TemperatureScale.Celsius, opts);
        }

        if (format.Equals("R", StringComparison.OrdinalIgnoreCase))
            return KelvinDelta.ToString(culture);

        return KelvinDelta.ToString(format, culture);
    }

    public override string ToString() => Humanize();
}

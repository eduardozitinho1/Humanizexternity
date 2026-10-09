namespace H13y.Measures;

/// <summary>
/// A temperature, stored internally as a number of kelvin.
/// </summary>
/// <param name="Kelvin">The temperature in kelvin.</param>
public readonly record struct Temperature(double Kelvin) : IComparable<Temperature>, IFormattable
{
    public static Temperature FromCelsius(double celsius) => new(celsius + 273.15);

    public static Temperature FromFahrenheit(double fahrenheit) =>
        new((fahrenheit - 32) * 5.0 / 9.0 + 273.15);

    public static Temperature FromKelvin(double kelvin) => new(kelvin);

    public double ToCelsius() => Kelvin - 273.15;

    public double ToFahrenheit() => (Kelvin - 273.15) * 9.0 / 5.0 + 32;

    public double ToKelvin() => Kelvin;

    public static Temperature operator +(Temperature left, Temperature right) =>
        new(left.Kelvin + right.Kelvin);

    public static Temperature operator -(Temperature left, Temperature right) =>
        new(left.Kelvin - right.Kelvin);

    public static bool operator <(Temperature left, Temperature right) =>
        left.Kelvin < right.Kelvin;

    public static bool operator <=(Temperature left, Temperature right) =>
        left.Kelvin <= right.Kelvin;

    public static bool operator >(Temperature left, Temperature right) =>
        left.Kelvin > right.Kelvin;

    public static bool operator >=(Temperature left, Temperature right) =>
        left.Kelvin >= right.Kelvin;

    public int CompareTo(Temperature other) => Kelvin.CompareTo(other.Kelvin);

    public string Humanize(
        TemperatureScale scale = TemperatureScale.Celsius,
        HumanizeOptions? options = null
    ) => TemperatureFormatter.Format(Kelvin, scale, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatTemperature(Kelvin, format, formatProvider);

    public override string ToString() => Humanize();
}

namespace H13y.Measures;

/// <summary>
/// A temperature, stored internally as a number of kelvin.
/// </summary>
/// <remarks>
/// Temperature is the only affine dimension in the library: the conversion between scales
/// is <c>base = value * factor + offset</c>, not a simple ratio. Because of that, the type
/// carries its own humanization entry point that takes a <see cref="TemperatureScale"/>
/// rather than relying on auto unit selection.
///
/// The type supports addition, subtraction, and comparison on the kelvin scale, matching the
/// behavior of the other measures. Arithmetic in kelvin is physically meaningful for thermal
/// calculations; converting the result back to Celsius or Fahrenheit is a display concern.
///
/// <code>
/// var t = Temperature.FromCelsius(25);
/// t.ToFahrenheit();       // 77
/// t.ToKelvin();           // 298.15
/// t.Humanize();           // "25 °C"
/// t.Humanize(TemperatureScale.Fahrenheit);  // "77 °F"
/// </code>
/// </remarks>
/// <param name="Kelvin">The temperature in kelvin.</param>
public readonly record struct Temperature(double Kelvin) : IComparable<Temperature>
{
    /// <summary>Creates a <see cref="Temperature"/> from a value in degrees Celsius.</summary>
    public static Temperature FromCelsius(double celsius) => new(celsius + 273.15);

    /// <summary>Creates a <see cref="Temperature"/> from a value in degrees Fahrenheit.</summary>
    public static Temperature FromFahrenheit(double fahrenheit)
        => new((fahrenheit - 32) * 5.0 / 9.0 + 273.15);

    /// <summary>Creates a <see cref="Temperature"/> from a value in kelvin.</summary>
    public static Temperature FromKelvin(double kelvin) => new(kelvin);

    /// <summary>Returns the temperature expressed in degrees Celsius.</summary>
    public double ToCelsius() => Kelvin - 273.15;

    /// <summary>Returns the temperature expressed in degrees Fahrenheit.</summary>
    public double ToFahrenheit() => (Kelvin - 273.15) * 9.0 / 5.0 + 32;

    /// <summary>Returns the temperature expressed in kelvin (the stored value).</summary>
    public double ToKelvin() => Kelvin;

    /// <summary>Adds two temperatures together on the kelvin scale.</summary>
    public static Temperature operator +(Temperature left, Temperature right)
        => new(left.Kelvin + right.Kelvin);

    /// <summary>Subtracts one temperature from another on the kelvin scale.</summary>
    public static Temperature operator -(Temperature left, Temperature right)
        => new(left.Kelvin - right.Kelvin);

    /// <summary>Compares two temperatures by kelvin value.</summary>
    public static bool operator <(Temperature left, Temperature right) => left.Kelvin < right.Kelvin;

    /// <summary>Compares two temperatures by kelvin value.</summary>
    public static bool operator <=(Temperature left, Temperature right) => left.Kelvin <= right.Kelvin;

    /// <summary>Compares two temperatures by kelvin value.</summary>
    public static bool operator >(Temperature left, Temperature right) => left.Kelvin > right.Kelvin;

    /// <summary>Compares two temperatures by kelvin value.</summary>
    public static bool operator >=(Temperature left, Temperature right) => left.Kelvin >= right.Kelvin;

    /// <summary>Compares this temperature to another by kelvin value.</summary>
    public int CompareTo(Temperature other) => Kelvin.CompareTo(other.Kelvin);

    /// <summary>
    /// Renders the temperature as a human-readable string such as "25 °C" or "77 °F".
    /// </summary>
    /// <param name="scale">The scale to display in. Defaults to <see cref="TemperatureScale.Celsius"/>.</param>
    /// <param name="options">Optional formatting options.</param>
    public string Humanize(TemperatureScale scale = TemperatureScale.Celsius, HumanizeOptions? options = null)
        => TemperatureFormatter.Format(Kelvin, scale, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default scale (Celsius) and default options.</summary>
    public override string ToString() => Humanize();
}

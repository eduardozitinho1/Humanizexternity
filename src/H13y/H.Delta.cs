using H13y.Measures;

namespace H13y;

public static partial class H
{
    public static string DeltaCelsius(double deltaCelsius, HumanizeOptions? options = null) =>
        TemperatureDelta
            .FromCelsius(deltaCelsius)
            .Humanize(TemperatureScale.Celsius, options);

    public static string DeltaFahrenheit(
        double deltaFahrenheit,
        HumanizeOptions? options = null
    ) =>
        TemperatureDelta
            .FromFahrenheit(deltaFahrenheit)
            .Humanize(TemperatureScale.Fahrenheit, options);

    public static string DeltaKelvin(double deltaKelvin, HumanizeOptions? options = null) =>
        TemperatureDelta
            .FromKelvin(deltaKelvin)
            .Humanize(TemperatureScale.Kelvin, options);
}

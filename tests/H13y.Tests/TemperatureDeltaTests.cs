using System.Globalization;
using System.Text.Json;
using H13y.Json;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class TemperatureDeltaTests
{
    [Fact]
    public void FromCelsius_to_kelvin_is_identity() =>
        Assert.Equal(10, TemperatureDelta.FromCelsius(10).ToKelvin(), precision: 6);

    [Fact]
    public void FromFahrenheit_converts_to_kelvin() =>
        Assert.Equal(10, TemperatureDelta.FromFahrenheit(18).ToKelvin(), precision: 6);

    [Fact]
    public void ToFahrenheit_from_kelvin() =>
        Assert.Equal(18, TemperatureDelta.FromKelvin(10).ToFahrenheit(), precision: 6);

    [Fact]
    public void Addition_sums_deltas() =>
        Assert.Equal(
            20,
            (TemperatureDelta.FromCelsius(10) + TemperatureDelta.FromCelsius(10)).ToKelvin(),
            precision: 6
        );

    [Fact]
    public void Subtraction_returns_difference() =>
        Assert.Equal(
            5,
            (TemperatureDelta.FromCelsius(15) - TemperatureDelta.FromCelsius(10)).ToKelvin(),
            precision: 6
        );

    [Fact]
    public void Unary_negation() =>
        Assert.Equal(-10, (-TemperatureDelta.FromCelsius(10)).ToKelvin(), precision: 6);

    [Fact]
    public void Comparison_orders_by_kelvin() =>
        Assert.True(TemperatureDelta.FromCelsius(20) > TemperatureDelta.FromCelsius(10));

    [Fact]
    public void Equality_across_scales() =>
        Assert.Equal(TemperatureDelta.FromCelsius(10), TemperatureDelta.FromFahrenheit(18));

    [Fact]
    public void Humanize_uses_delta_prefix() =>
        Assert.Equal("\u039410 \u00b0C", TemperatureDelta.FromCelsius(10).Humanize());

    [Fact]
    public void Humanize_fahrenheit() =>
        Assert.Equal(
            "\u039418 \u00b0F",
            TemperatureDelta.FromFahrenheit(18).Humanize(TemperatureScale.Fahrenheit)
        );

    [Fact]
    public void Humanize_kelvin() =>
        Assert.Equal(
            "\u039410 K",
            TemperatureDelta.FromKelvin(10).Humanize(TemperatureScale.Kelvin)
        );

    [Fact]
    public void Humanize_no_space() =>
        Assert.Equal(
            "\u039410\u00b0C",
            TemperatureDelta
                .FromCelsius(10)
                .Humanize(
                    TemperatureScale.Celsius,
                    new HumanizeOptions { SpaceBetweenValueAndUnit = false }
                )
        );

    [Fact]
    public void Temperature_plus_delta_returns_absolute()
    {
        var t = Temperature.FromCelsius(20);
        var d = TemperatureDelta.FromCelsius(10);
        Assert.Equal(30, (t + d).ToCelsius(), precision: 6);
    }

    [Fact]
    public void Temperature_minus_delta_returns_absolute()
    {
        var t = Temperature.FromCelsius(20);
        var d = TemperatureDelta.FromCelsius(5);
        Assert.Equal(15, (t - d).ToCelsius(), precision: 6);
    }

    [Fact]
    public void Difference_returns_delta()
    {
        var a = Temperature.FromCelsius(20);
        var b = Temperature.FromCelsius(30);
        Assert.Equal(10, b.Difference(a).ToKelvin(), precision: 6);
    }

    [Fact]
    public void Add_method_returns_absolute()
    {
        var t = Temperature.FromCelsius(20);
        var d = TemperatureDelta.FromCelsius(10);
        Assert.Equal(30, t.Add(d).ToCelsius(), precision: 6);
    }

    [Fact]
    public void Subtract_method_returns_absolute()
    {
        var t = Temperature.FromCelsius(20);
        var d = TemperatureDelta.FromCelsius(5);
        Assert.Equal(15, t.Subtract(d).ToCelsius(), precision: 6);
    }

    [Fact]
    public void Static_difference_between_two_temperatures() =>
        Assert.Equal(
            10,
            TemperatureDelta
                .Difference(Temperature.FromCelsius(20), Temperature.FromCelsius(30))
                .ToKelvin(),
            precision: 6
        );

    [Fact]
    public void H_DeltaCelsius() => Assert.Equal("\u039410 \u00b0C", H.DeltaCelsius(10));

    [Fact]
    public void H_DeltaFahrenheit() => Assert.Equal("\u039418 \u00b0F", H.DeltaFahrenheit(18));

    [Fact]
    public void H_DeltaKelvin() => Assert.Equal("\u039410 K", H.DeltaKelvin(10));

    [Fact]
    public void Culture_is_honored()
    {
        var opts = new HumanizeOptions { Culture = new CultureInfo("pt-BR") };
        Assert.Equal("\u039410,5 \u00b0C", H.DeltaCelsius(10.5, opts));
    }

    [Fact]
    public void Delta_implements_IFormattable()
    {
        var d = TemperatureDelta.FromCelsius(10);
        Assert.Equal("\u039410 \u00b0C", d.ToString("H", CultureInfo.InvariantCulture));
        Assert.Equal("10", d.ToString("R", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Delta_NaN() =>
        Assert.Equal("NaN", TemperatureDelta.FromKelvin(double.NaN).Humanize());

    [Fact]
    public void Delta_positive_infinity() =>
        Assert.Equal("\u221e", TemperatureDelta.FromKelvin(double.PositiveInfinity).Humanize());

    [Fact]
    public void Json_round_trip()
    {
        var original = TemperatureDelta.FromCelsius(10);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<TemperatureDelta>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }
}

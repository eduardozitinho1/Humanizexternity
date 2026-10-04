using Xunit;
using System.Globalization;

namespace H13y.Tests;

public class ConversionTests
{
    [Fact]
    public void Grams_to_kilograms()
    {
        var result = H.Convert(1500, Units.Mass.Gram, Units.Mass.Kilogram);
        Assert.Equal(1.5, result, precision: 6);
    }

    [Fact]
    public void Kilograms_to_grams()
    {
        var result = H.Convert(1.5, Units.Mass.Kilogram, Units.Mass.Gram);
        Assert.Equal(1500, result, precision: 6);
    }

    [Fact]
    public void Bytes_to_kilobytes()
    {
        var result = H.Convert(1024, Units.Data.Byte, Units.Data.Kilobyte);
        Assert.Equal(1.0, result, precision: 6);
    }

    [Fact]
    public void Gigabytes_to_megabytes()
    {
        var result = H.Convert(1, Units.Data.Gigabyte, Units.Data.Megabyte);
        Assert.Equal(1024, result, precision: 6);
    }

    [Fact]
    public void Meters_to_kilometers()
    {
        var result = H.Convert(1500, Units.Length.Meter, Units.Length.Kilometer);
        Assert.Equal(1.5, result, precision: 6);
    }

    [Fact]
    public void Celsius_to_kelvin()
    {
        var result = H.Convert(0, Units.Temperature.Celsius, Units.Temperature.Kelvin);
        Assert.Equal(273.15, result, precision: 6);
    }

    [Fact]
    public void Kelvin_to_celsius()
    {
        var result = H.Convert(273.15, Units.Temperature.Kelvin, Units.Temperature.Celsius);
        Assert.Equal(0, result, precision: 6);
    }

    [Fact]
    public void Celsius_to_fahrenheit_freezing()
    {
        var result = H.Convert(0, Units.Temperature.Celsius, Units.Temperature.Fahrenheit);
        Assert.Equal(32, result, precision: 6);
    }

    [Fact]
    public void Celsius_to_fahrenheit_boiling()
    {
        var result = H.Convert(100, Units.Temperature.Celsius, Units.Temperature.Fahrenheit);
        Assert.Equal(212, result, precision: 6);
    }

    [Fact]
    public void Fahrenheit_to_celsius_minus_40()
    {
        var result = H.Convert(-40, Units.Temperature.Fahrenheit, Units.Temperature.Celsius);
        Assert.Equal(-40, result, precision: 6);
    }

    [Fact]
    public void Same_unit_returns_same_value()
    {
        var result = H.Convert(42, Units.Mass.Gram, Units.Mass.Gram);
        Assert.Equal(42, result, precision: 6);
    }

    [Fact]
    public void Incompatible_dimensions_throw()
    {
        Assert.Throws<ArgumentException>(() =>
            H.Convert(1, Units.Data.Byte, Units.Mass.Gram));
    }

    [Fact]
    public void Parse_and_convert_string_to_target_unit()
    {
        var result = H.Convert("1.5 GB", Units.Data.Megabyte, CultureInfo.InvariantCulture);
        Assert.Equal(1536, result, precision: 6);
    }

    [Fact]
    public void Parse_and_convert_string_to_kilograms()
    {
        var result = H.Convert("1500 g", Units.Mass.Kilogram, CultureInfo.InvariantCulture);
        Assert.Equal(1.5, result, precision: 6);
    }

    [Fact]
    public void Parse_and_convert_with_iec_symbol()
    {
        var result = H.Convert("1 GiB", Units.Data.Megabyte, CultureInfo.InvariantCulture);
        Assert.Equal(1024, result, precision: 6);
    }

    [Fact]
    public void Parse_and_convert_incompatible_throws()
    {
        Assert.Throws<ArgumentException>(() =>
            H.Convert("1 kg", Units.Data.Byte, CultureInfo.InvariantCulture));
    }
}

using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class TypedTryParseTests
{
    [Fact]
    public void TryParseDataSize_success()
    {
        Assert.True(H.TryParseDataSize("1.5 GB", out var size));
        Assert.Equal(1536L * 1024 * 1024, size.Bytes);
    }

    [Fact]
    public void TryParseDataSize_accepts_iec()
    {
        Assert.True(H.TryParseDataSize("1 GiB", out var size));
        Assert.Equal(1024L * 1024 * 1024, size.Bytes);
    }

    [Fact]
    public void TryParseDataSize_rejects_other_dimension()
    {
        Assert.False(H.TryParseDataSize("1.5 kg", out _));
    }

    [Fact]
    public void TryParseDataSize_rejects_invalid()
    {
        Assert.False(H.TryParseDataSize("garbage", out _));
    }

    [Fact]
    public void TryParseMass_success()
    {
        Assert.True(H.TryParseMass("1500 g", out var mass));
        Assert.Equal(1500, mass.Grams, precision: 6);
    }

    [Fact]
    public void TryParseMass_kilograms()
    {
        Assert.True(H.TryParseMass("1.5 kg", out var mass));
        Assert.Equal(1500, mass.Grams, precision: 6);
    }

    [Fact]
    public void TryParseMass_rejects_other_dimension()
    {
        Assert.False(H.TryParseMass("1 GB", out _));
    }

    [Fact]
    public void TryParseLength_success()
    {
        Assert.True(H.TryParseLength("1.5 km", out var length));
        Assert.Equal(1500, length.Meters, precision: 6);
    }

    [Fact]
    public void TryParseLength_rejects_other_dimension()
    {
        Assert.False(H.TryParseLength("1 kg", out _));
    }

    [Fact]
    public void TryParseDuration_compound()
    {
        Assert.True(H.TryParseDuration("1h30min", out var d));
        Assert.Equal(5400, d.Seconds, precision: 6);
    }

    [Fact]
    public void TryParseDuration_colon()
    {
        Assert.True(H.TryParseDuration("1:30:45", out var d));
        Assert.Equal(5445, d.Seconds, precision: 6);
    }

    [Fact]
    public void TryParseDuration_rejects_other_dimension()
    {
        Assert.False(H.TryParseDuration("1 m", out _));
    }

    [Fact]
    public void TryParseVolume_success()
    {
        Assert.True(H.TryParseVolume("500 ml", out var v));
        Assert.Equal(0.5, v.Liters, precision: 6);
    }

    [Fact]
    public void TryParseVolume_rejects_other_dimension()
    {
        Assert.False(H.TryParseVolume("1 kg", out _));
    }

    [Fact]
    public void TryParseArea_success()
    {
        Assert.True(H.TryParseArea("1 ha", out var a));
        Assert.Equal(10_000, a.SquareMeters, precision: 6);
    }

    [Fact]
    public void TryParseArea_rejects_other_dimension()
    {
        Assert.False(H.TryParseArea("1 GB", out _));
    }

    [Fact]
    public void TryParseTemperature_celsius()
    {
        Assert.True(H.TryParseTemperature("25 °C", out var t));
        Assert.Equal(298.15, t.Kelvin, precision: 6);
    }

    [Fact]
    public void TryParseTemperature_fahrenheit()
    {
        Assert.True(H.TryParseTemperature("77 °F", out var t));
        Assert.Equal(298.15, t.Kelvin, precision: 6);
    }

    [Fact]
    public void TryParseTemperature_kelvin()
    {
        Assert.True(H.TryParseTemperature("300 K", out var t));
        Assert.Equal(300, t.Kelvin, precision: 6);
    }

    [Fact]
    public void TryParseTemperature_rejects_other_dimension()
    {
        Assert.False(H.TryParseTemperature("1 GB", out _));
    }

    [Fact]
    public void TryParse_returns_default_on_failure()
    {
        Assert.False(H.TryParseMass("invalid", out var mass));
        Assert.Equal(default, mass);
    }
}

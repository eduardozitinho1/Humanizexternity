using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class TryParseFamilyTests
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    [Fact]
    public void TryParseSpeed_success()
    {
        Assert.True(H.TryParseSpeed("100 km/h", Inv, out var s));
        Assert.Equal(100.0 * 1000 / 3600, s.MetersPerSecond, precision: 6);
    }

    [Fact]
    public void TryParseSpeed_rejects_other_dimension() =>
        Assert.False(H.TryParseSpeed("1.5 kg", Inv, out _));

    [Fact]
    public void TryParseEnergy_success()
    {
        Assert.True(H.TryParseEnergy("1.5 kWh", Inv, out var e));
        Assert.Equal(5_400_000, e.Joules, precision: 3);
    }

    [Fact]
    public void TryParseEnergy_rejects_other_dimension() =>
        Assert.False(H.TryParseEnergy("1 GB", Inv, out _));

    [Fact]
    public void TryParsePower_success()
    {
        Assert.True(H.TryParsePower("2 MW", Inv, out var p));
        Assert.Equal(2_000_000, p.Watts, precision: 3);
    }

    [Fact]
    public void TryParsePower_rejects_other_dimension() =>
        Assert.False(H.TryParsePower("2 kWh", Inv, out _));

    [Fact]
    public void TryParsePressure_success()
    {
        Assert.True(H.TryParsePressure("2 bar", Inv, out var p));
        Assert.Equal(200_000, p.Pascals, precision: 3);
    }

    [Fact]
    public void TryParsePressure_rejects_other_dimension() =>
        Assert.False(H.TryParsePressure("100 km/h", Inv, out _));

    [Fact]
    public void TryParseFrequency_success()
    {
        Assert.True(H.TryParseFrequency("2.4 GHz", Inv, out var f));
        Assert.Equal(2_400_000_000, f.Hertz, precision: 3);
    }

    [Fact]
    public void TryParseFrequency_rejects_other_dimension() =>
        Assert.False(H.TryParseFrequency("1.5 kg", Inv, out _));

    [Fact]
    public void TryParseAngle_success()
    {
        Assert.True(H.TryParseAngle("90 deg", Inv, out var a));
        Assert.Equal(Math.PI / 2, a.Radians, precision: 6);
    }

    [Fact]
    public void TryParseAngle_accepts_degree_symbol()
    {
        Assert.True(H.TryParseAngle("180\u00b0", Inv, out var a));
        Assert.Equal(Math.PI, a.Radians, precision: 6);
    }

    [Fact]
    public void TryParseAngle_rejects_other_dimension() =>
        Assert.False(H.TryParseAngle("1.5 kg", Inv, out _));

    [Fact]
    public void TryParseBits_success()
    {
        Assert.True(H.TryParseBits("1 Gb", Inv, out var b));
        Assert.Equal(1_000_000_000, b.Value, precision: 3);
    }

    [Fact]
    public void TryParseBits_rejects_byte_dimension() =>
        Assert.False(H.TryParseBits("1 GB", Inv, out _));

    [Fact]
    public void TryParseDataRate_success()
    {
        Assert.True(H.TryParseDataRate("100 Mbps", Inv, out var r));
        Assert.Equal(100_000_000, r.BitsPerSecond, precision: 3);
    }

    [Fact]
    public void TryParseDataRate_rejects_other_dimension() =>
        Assert.False(H.TryParseDataRate("1.5 kg", Inv, out _));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("garbage")]
    [InlineData("1 zzz")]
    public void TryParse_never_throws(string? input)
    {
        Assert.False(H.TryParseSpeed(input!, Inv, out _));
        Assert.False(H.TryParseEnergy(input!, Inv, out _));
        Assert.False(H.TryParsePower(input!, Inv, out _));
        Assert.False(H.TryParsePressure(input!, Inv, out _));
        Assert.False(H.TryParseFrequency(input!, Inv, out _));
        Assert.False(H.TryParseAngle(input!, Inv, out _));
        Assert.False(H.TryParseBits(input!, Inv, out _));
        Assert.False(H.TryParseDataRate(input!, Inv, out _));
    }
}

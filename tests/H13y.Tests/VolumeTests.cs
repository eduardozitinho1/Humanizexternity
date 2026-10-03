using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class VolumeTests
{
    [Fact]
    public void FromLiters_converts_to_liters()
    {
        var v = Volume.FromLiters(1.5);
        Assert.Equal(1.5, v.Liters);
    }

    [Fact]
    public void FromMilliliters_converts_to_liters()
    {
        var v = Volume.FromMilliliters(500);
        Assert.Equal(0.5, v.Liters);
    }

    [Fact]
    public void FromCubicMeters_converts_to_liters()
    {
        var v = Volume.FromCubicMeters(1);
        Assert.Equal(1000, v.Liters);
    }

    [Fact]
    public void ToMilliliters_converts_correctly()
    {
        var v = Volume.FromLiters(1.5);
        Assert.Equal(1500, v.ToMilliliters());
    }

    [Theory]
    [InlineData(0.5, "500 ml")]
    [InlineData(1.5, "1.5 l")]
    [InlineData(1500, "1.5 m3")]
    public void Humanize_returns_correct_text(double liters, string expected)
    {
        var v = Volume.FromLiters(liters);
        Assert.Equal(expected, v.Humanize());
    }

    [Fact]
    public void H_Liters_returns_humanized_string()
    {
        Assert.Equal("1.5 l", H.Liters(1.5));
    }

    [Theory]
    [InlineData("500 ml", 0.5)]
    [InlineData("1.5 l", 1.5)]
    [InlineData("1 m3", 1000)]
    public void Parse_volume_units(string input, double expectedLiters)
    {
        var m = H.Parse(input);
        var v = Volume.FromLiters(m.ToBase());
        Assert.Equal(expectedLiters, v.Liters, precision: 6);
    }
}

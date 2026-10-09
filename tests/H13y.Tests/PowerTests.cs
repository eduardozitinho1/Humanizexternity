using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class PowerTests
{
    [Fact]
    public void FromKilowatts_to_watts() =>
        Assert.Equal(1500, Power.FromKilowatts(1.5).Watts, precision: 6);

    [Fact]
    public void FromHorsepower_to_watts() =>
        Assert.Equal(745.7, Power.FromHorsepower(1).Watts, precision: 1);

    [Theory]
    [InlineData(500, "500 W")]
    [InlineData(1500, "1.5 kW")]
    [InlineData(1_500_000, "1.5 MW")]
    [InlineData(2_000_000_000, "2 GW")]
    public void Humanize_scales(double watts, string expected) =>
        Assert.Equal(expected, Power.FromWatts(watts).Humanize());

    [Fact]
    public void Zero_humanizes_in_base_unit() => Assert.Equal("0 W", Power.FromWatts(0).Humanize());

    [Theory]
    [InlineData("500 W", 500)]
    [InlineData("1.5 kW", 1.5)]
    [InlineData("2 MW", 2)]
    [InlineData("1 GW", 1)]
    [InlineData("100 hp", 100)]
    [InlineData("1 kilowatt", 1)]
    public void Parse_variants(string input, double expectedValue)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Power, m.Unit.Dimension);
        Assert.Equal(expectedValue, m.Value, precision: 6);
    }

    [Fact]
    public void H_Kilowatts() => Assert.Equal("1.5 kW", H.Kilowatts(1.5));

    [Fact]
    public void Conversion_kw_to_w() =>
        Assert.Equal(1500, H.Convert(1.5, Units.Power.Kilowatt, Units.Power.Watt), precision: 6);

    [Fact]
    public void Comparison_orders_by_watts() =>
        Assert.True(Power.FromMegawatts(1) > Power.FromKilowatts(1));

    [Fact]
    public void Equality_across_units() =>
        Assert.Equal(Power.FromWatts(1000), Power.FromKilowatts(1));
}

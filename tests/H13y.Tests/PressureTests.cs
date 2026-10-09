using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class PressureTests
{
    [Fact]
    public void FromKilopascals_to_pascals() =>
        Assert.Equal(1500, Pressure.FromKilopascals(1.5).Pascals, precision: 6);

    [Fact]
    public void FromBars_to_pascals() =>
        Assert.Equal(100_000, Pressure.FromBars(1).Pascals, precision: 6);

    [Fact]
    public void FromAtmospheres_to_pascals() =>
        Assert.Equal(101_325, Pressure.FromAtmospheres(1).Pascals, precision: 6);

    [Theory]
    [InlineData(500, "500 Pa")]
    [InlineData(5000, "5 kPa")]
    [InlineData(150_000, "1.5 bar")]
    [InlineData(2_000_000, "2 MPa")]
    public void Humanize_scales(double pa, string expected) =>
        Assert.Equal(expected, Pressure.FromPascals(pa).Humanize());

    [Fact]
    public void Zero_humanizes_in_base_unit() =>
        Assert.Equal("0 Pa", Pressure.FromPascals(0).Humanize());

    [Theory]
    [InlineData("500 Pa", 500)]
    [InlineData("5 kPa", 5)]
    [InlineData("2 MPa", 2)]
    [InlineData("1 bar", 1)]
    [InlineData("14.7 psi", 14.7)]
    [InlineData("1 atm", 1)]
    public void Parse_variants(string input, double expectedValue)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Pressure, m.Unit.Dimension);
        Assert.Equal(expectedValue, m.Value, precision: 3);
    }

    [Fact]
    public void H_Bars() => Assert.Equal("1.5 bar", H.Bars(1.5));

    [Fact]
    public void Conversion_atm_to_pa() =>
        Assert.Equal(
            101_325,
            H.Convert(1, Units.Pressure.Atmosphere, Units.Pressure.Pascal),
            precision: 3
        );

    [Fact]
    public void Comparison_orders_by_pascals() =>
        Assert.True(Pressure.FromBars(1) > Pressure.FromKilopascals(1));

    [Fact]
    public void Equality_across_units() =>
        Assert.Equal(Pressure.FromPascals(100_000), Pressure.FromBars(1));
}

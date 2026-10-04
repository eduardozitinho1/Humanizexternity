using H13y.Measures;
using Xunit;
using System.Globalization;

namespace H13y.Tests;

public class AngleTests
{
    [Fact]
    public void FromDegrees_to_radians()
        => Assert.Equal(Math.PI, Angle.FromDegrees(180).Radians, precision: 6);

    [Fact]
    public void FromTurns_to_radians()
        => Assert.Equal(2 * Math.PI, Angle.FromTurns(1).Radians, precision: 6);

    [Theory]
    [InlineData(0, "0 deg")]
    [InlineData(45, "45 deg")]
    [InlineData(90, "90 deg")]
    [InlineData(180, "180 deg")]
    [InlineData(360, "360 deg")]
    public void Humanize_degrees(double deg, string expected)
        => Assert.Equal(expected, Angle.FromDegrees(deg).Humanize());

    [Fact]
    public void Humanize_pi_radians_as_180_deg()
        => Assert.Equal("180 deg", Angle.FromRadians(Math.PI).Humanize());

    [Theory]
    [InlineData("90 deg", 90)]
    [InlineData("45 deg", 45)]
    [InlineData("1 rad", 1)]
    [InlineData("180°", 180)]
    [InlineData("90°", 90)]
    public void Parse_variants(string input, double expectedValue)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Angle, m.Unit.Dimension);
        Assert.Equal(expectedValue, m.Value, precision: 3);
    }

    [Fact]
    public void H_Degrees()
        => Assert.Equal("45 deg", H.Degrees(45));

    [Fact]
    public void Conversion_deg_to_rad()
        => Assert.Equal(Math.PI / 2, H.Convert(90, Units.Angle.Degree, Units.Angle.Radian), precision: 6);

    [Fact]
    public void Comparison_orders_by_radians()
        => Assert.True(Angle.FromDegrees(180) > Angle.FromDegrees(45));

    [Fact]
    public void Equality_across_units()
        => Assert.Equal(Angle.FromDegrees(180), Angle.FromRadians(Math.PI));
}

using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class EnergyTests
{
    [Fact]
    public void FromKilojoules_to_joules()
        => Assert.Equal(1500, Energy.FromKilojoules(1.5).Joules, precision: 6);

    [Fact]
    public void FromKilowattHours_to_joules()
        => Assert.Equal(3_600_000, Energy.FromKilowattHours(1).Joules, precision: 6);

    [Fact]
    public void FromKilocalories_to_joules()
        => Assert.Equal(4184, Energy.FromKilocalories(1).Joules, precision: 6);

    [Fact]
    public void ToKilowattHours_round_trips()
        => Assert.Equal(1, Energy.FromJoules(3_600_000).ToKilowattHours(), precision: 6);

    [Theory]
    [InlineData(500, "500 J")]
    [InlineData(1500, "1.5 kJ")]
    [InlineData(1_500_000, "1.5 MJ")]
    [InlineData(3_600_000, "1 kWh")]
    public void Humanize_scales(double joules, string expected)
        => Assert.Equal(expected, Energy.FromJoules(joules).Humanize());

    [Fact]
    public void Kilocalorie_not_auto_selected()
    {
        // 4184 J = 1 kcal, but auto-select shows kJ
        Assert.Equal("4.2 kJ", Energy.FromJoules(4184).Humanize());
    }

    [Fact]
    public void Zero_humanizes_in_base_unit()
        => Assert.Equal("0 J", Energy.FromJoules(0).Humanize());

    [Theory]
    [InlineData("1500 J", 1500)]
    [InlineData("1.5 kJ", 1.5)]
    [InlineData("2 MJ", 2)]
    [InlineData("500 cal", 500)]
    [InlineData("100 kcal", 100)]
    [InlineData("1 Wh", 1)]
    [InlineData("1 kWh", 1)]
    [InlineData("1 kilojoule", 1)]
    [InlineData("5 calories", 5)]
    public void Parse_variants(string input, double expectedValue)
    {
        var m = H.Parse(input);
        Assert.Equal(Dimension.Energy, m.Unit.Dimension);
        Assert.Equal(expectedValue, m.Value, precision: 6);
    }

    [Fact]
    public void H_Kilojoules()
        => Assert.Equal("1.5 kJ", H.Kilojoules(1.5));

    [Fact]
    public void H_KilowattHours()
        => Assert.Equal("1 kWh", H.KilowattHours(1));

    [Fact]
    public void Conversion_kwh_to_mj()
        => Assert.Equal(3.6, H.Convert(1, Units.Energy.KilowattHour, Units.Energy.Megajoule), precision: 6);

    [Fact]
    public void Comparison_orders_by_joules()
        => Assert.True(Energy.FromMegajoules(1) > Energy.FromKilojoules(1));

    [Fact]
    public void Equality_across_units()
        => Assert.Equal(Energy.FromJoules(1000), Energy.FromKilojoules(1));
}

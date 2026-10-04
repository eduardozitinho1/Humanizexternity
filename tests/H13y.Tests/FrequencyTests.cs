using H13y.Measures;
using Xunit;
using System.Globalization;

namespace H13y.Tests;

public class FrequencyTests
{
    [Fact]
    public void FromKilohertz_to_hertz()
        => Assert.Equal(1500, Frequency.FromKilohertz(1.5).Hertz, precision: 6);

    [Theory]
    [InlineData(500, "500 Hz")]
    [InlineData(1500, "1.5 kHz")]
    [InlineData(1_500_000, "1.5 MHz")]
    [InlineData(2_000_000_000, "2 GHz")]
    public void Humanize_scales(double hz, string expected)
        => Assert.Equal(expected, Frequency.FromHertz(hz).Humanize());

    [Fact]
    public void Zero_humanizes_in_base_unit()
        => Assert.Equal("0 Hz", Frequency.FromHertz(0).Humanize());

    [Theory]
    [InlineData("500 Hz", 500)]
    [InlineData("1.5 kHz", 1.5)]
    [InlineData("2.4 MHz", 2.4)]
    [InlineData("5 GHz", 5)]
    public void Parse_variants(string input, double expectedValue)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Frequency, m.Unit.Dimension);
        Assert.Equal(expectedValue, m.Value, precision: 6);
    }

    [Fact]
    public void H_Gigahertz()
        => Assert.Equal("2.4 GHz", H.Gigahertz(2.4));

    [Fact]
    public void Conversion_mhz_to_hz()
        => Assert.Equal(1_000_000, H.Convert(1, Units.Frequency.Megahertz, Units.Frequency.Hertz), precision: 3);

    [Fact]
    public void Comparison_orders_by_hertz()
        => Assert.True(Frequency.FromGigahertz(1) > Frequency.FromMegahertz(1));

    [Fact]
    public void Equality_across_units()
        => Assert.Equal(Frequency.FromHertz(1000), Frequency.FromKilohertz(1));
}

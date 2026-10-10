using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class DataRateTests
{
    [Fact]
    public void FromMegabits_to_bits() =>
        Assert.Equal(1_000_000, DataRate.FromMegabitsPerSecond(1).BitsPerSecond, precision: 6);

    [Fact]
    public void FromMegabytes_to_bits() =>
        Assert.Equal(8_000_000, DataRate.FromMegabytesPerSecond(1).BitsPerSecond, precision: 6);

    [Theory]
    [InlineData(1_000, "1 Kbps")]
    [InlineData(1_000_000, "1 Mbps")]
    [InlineData(100_000_000, "100 Mbps")]
    [InlineData(1_000_000_000, "1 Gbps")]
    [InlineData(1_000_000_000_000, "1 Tbps")]
    public void Humanize_scales(double bps, string expected) =>
        Assert.Equal(expected, DataRate.FromBitsPerSecond(bps).Humanize());

    [Fact]
    public void Zero_humanizes_in_bps() =>
        Assert.Equal("0 bps", DataRate.FromBitsPerSecond(0).Humanize());

    [Fact]
    public void Byte_per_second_shows_as_mbps()
    {
        var rate = DataRate.FromBytesPerSecond(1);
        Assert.Equal("8 bps", rate.Humanize());
    }

    [Theory]
    [InlineData("100 Mbps", 100_000_000)]
    [InlineData("1 Gbps", 1_000_000_000)]
    [InlineData("10 MB/s", 80_000_000)]
    [InlineData("1 GB/s", 8_000_000_000)]
    public void Parse_variants(string input, double expectedBps)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.DataRate, m.Unit.Dimension);
        Assert.Equal(expectedBps, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Convert_mbps_to_mb_per_second() =>
        Assert.Equal(
            12.5,
            H.Convert(100, Units.DataRate.MegabitPerSecond, Units.DataRate.MegabytePerSecond),
            precision: 6
        );

    [Fact]
    public void Comparison_orders_by_bps() =>
        Assert.True(
            DataRate.FromGigabitsPerSecond(1) > DataRate.FromMegabitsPerSecond(1)
        );

    [Fact]
    public void Equality_across_units() =>
        Assert.Equal(
            DataRate.FromBitsPerSecond(1_000_000),
            DataRate.FromMegabitsPerSecond(1)
        );

    [Fact]
    public void FullName_uses_rate_names()
    {
        var opts = new HumanizeOptions { UnitStyle = UnitStyle.FullName };
        Assert.Equal("100 megabits per second", DataRate.FromMegabitsPerSecond(100).Humanize(opts));
        Assert.Equal("1 gigabit per second", DataRate.FromGigabitsPerSecond(1).Humanize(opts));
    }

    [Fact]
    public void H_MegabitsPerSecond() => Assert.Equal("100 Mbps", H.MegabitsPerSecond(100));

    [Fact]
    public void H_GigabitsPerSecond() => Assert.Equal("1 Gbps", H.GigabitsPerSecond(1));
}

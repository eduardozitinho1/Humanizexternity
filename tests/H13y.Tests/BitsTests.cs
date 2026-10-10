using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class BitsTests
{
    [Fact]
    public void FromKilobits_to_bits() =>
        Assert.Equal(1500, Bits.FromKilobits(1.5).Value, precision: 6);

    [Fact]
    public void FromGigabits_to_bits() =>
        Assert.Equal(1_000_000_000, Bits.FromGigabits(1).Value, precision: 6);

    [Theory]
    [InlineData(500, "500 b")]
    [InlineData(1000, "1 Kb")]
    [InlineData(1_500_000, "1.5 Mb")]
    [InlineData(1_000_000_000, "1 Gb")]
    [InlineData(1_000_000_000_000, "1 Tb")]
    public void Humanize_scales(double bits, string expected) =>
        Assert.Equal(expected, Bits.FromBits(bits).Humanize());

    [Fact]
    public void Zero_humanizes_in_base_unit() => Assert.Equal("0 b", Bits.FromBits(0).Humanize());

    [Theory]
    [InlineData("500 b", 500)]
    [InlineData("1 Kb", 1000)]
    [InlineData("100 Mb", 100_000_000)]
    [InlineData("1 Gb", 1_000_000_000)]
    [InlineData("1 Gbit", 1_000_000_000)]
    [InlineData("1 gigabit", 1_000_000_000)]
    [InlineData("1 kilobit", 1000)]
    public void Parse_variants(string input, double expected)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Bits, m.Unit.Dimension);
        Assert.Equal(expected, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Byte_and_bit_are_case_sensitive()
    {
        var b = H.Parse("1 B", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Data, b.Unit.Dimension);

        var bit = H.Parse("1 b", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Bits, bit.Unit.Dimension);
    }

    [Fact]
    public void H_Bits_facade() => Assert.Equal("1 Gb", H.Bits(1_000_000_000));

    [Fact]
    public void Comparison_orders_by_bits() =>
        Assert.True(Bits.FromGigabits(1) > Bits.FromMegabits(1));

    [Fact]
    public void Equality_across_units() => Assert.Equal(Bits.FromBits(1000), Bits.FromKilobits(1));

    [Fact]
    public void FullName_uses_bits_names()
    {
        var opts = new HumanizeOptions { UnitStyle = UnitStyle.FullName };
        Assert.Equal("1 gigabit", Bits.FromGigabits(1).Humanize(opts));
        Assert.Equal("2 gigabits", Bits.FromGigabits(2).Humanize(opts));
        Assert.Equal("1 bit", Bits.FromBits(1).Humanize(opts));
    }
}

using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class CompactTests
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(999, "999")]
    [InlineData(1000, "1K")]
    [InlineData(1234, "1.2K")]
    [InlineData(999_999, "1M")]
    [InlineData(1_500_000, "1.5M")]
    [InlineData(2_300_000_000, "2.3B")]
    [InlineData(1_000_000_000_000, "1T")]
    public void Compact_short_style(double value, string expected) =>
        Assert.Equal(expected, H.Compact(value));

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1_500, "1.5 thousand")]
    [InlineData(1_500_000, "1.5 million")]
    [InlineData(2_300_000_000, "2.3 billion")]
    [InlineData(1_000_000_000_000, "1 trillion")]
    public void CompactWords_long_style(double value, string expected) =>
        Assert.Equal(expected, H.CompactWords(value));

    [Theory]
    [InlineData(-1234, "-1.2K")]
    [InlineData(-1_500_000, "-1.5M")]
    public void Compact_supports_negative_values(double value, string expected) =>
        Assert.Equal(expected, H.Compact(value));

    [Fact]
    public void Compact_honors_decimal_separator_via_culture()
    {
        var opts = new HumanizeOptions { Culture = new CultureInfo("pt-BR") };
        Assert.Equal("1,5M", H.Compact(1_500_000, opts));
    }

    [Fact]
    public void Compact_honors_max_decimals()
    {
        var opts = new HumanizeOptions { MaxDecimals = 2 };
        Assert.Equal("1.23K", H.Compact(1234, opts));
    }

    [Fact]
    public void Compact_handles_NaN() => Assert.Equal("NaN", H.Compact(double.NaN));

    [Fact]
    public void Compact_handles_infinity()
    {
        Assert.Equal("∞", H.Compact(double.PositiveInfinity));
        Assert.Equal("-∞", H.Compact(double.NegativeInfinity));
    }

    [Fact]
    public void Compact_promotes_rounded_overflow()
    {
        // 999_999 would round to 1000K, so it promotes to 1M.
        Assert.Equal("1M", H.Compact(999_999));
    }
}

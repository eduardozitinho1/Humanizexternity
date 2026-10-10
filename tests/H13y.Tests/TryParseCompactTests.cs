using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class TryParseCompactTests
{
    [Theory]
    [InlineData("1.5K", 1_500)]
    [InlineData("1.5M", 1_500_000)]
    [InlineData("1.5B", 1_500_000_000)]
    [InlineData("1T", 1_000_000_000_000)]
    [InlineData("999", 999)]
    public void Short_suffixes_parse(string input, double expected)
    {
        Assert.True(H.TryParseCompact(input, out var value));
        Assert.Equal(expected, value, precision: 6);
    }

    [Theory]
    [InlineData("1.5 thousand", 1_500)]
    [InlineData("1.5 million", 1_500_000)]
    [InlineData("1.5 billion", 1_500_000_000)]
    [InlineData("1.5 trillion", 1_500_000_000_000)]
    public void Long_suffixes_parse(string input, double expected)
    {
        Assert.True(H.TryParseCompact(input, out var value));
        Assert.Equal(expected, value, precision: 6);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("K")]
    [InlineData("M")]
    [InlineData("not a number")]
    [InlineData("1.5X")]
    public void Invalid_input_returns_false(string input) =>
        Assert.False(H.TryParseCompact(input, out _));

    [Fact]
    public void Comma_culture_accepts_comma_decimal()
    {
        var opts = new HumanizeOptions { Culture = new CultureInfo("pt-BR") };
        Assert.True(H.TryParseCompact("1,5M", out var value, opts));
        Assert.Equal(1_500_000, value, precision: 6);
    }

    [Fact]
    public void Null_returns_false() => Assert.False(H.TryParseCompact(null!, out _));

    [Fact]
    public void Round_trip_short()
    {
        for (double v = 1_000; v <= 1_000_000_000; v *= 10)
        {
            var text = H.Compact(v);
            Assert.True(H.TryParseCompact(text, out var parsed));
            Assert.Equal(v, parsed, precision: 0);
        }
    }

    [Fact]
    public void Round_trip_long()
    {
        for (double v = 1_000; v <= 1_000_000_000_000; v *= 10)
        {
            var text = H.CompactWords(v);
            Assert.True(H.TryParseCompact(text, out var parsed));
            Assert.Equal(v, parsed, precision: 0);
        }
    }
}

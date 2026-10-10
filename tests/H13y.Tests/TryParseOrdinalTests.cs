using Xunit;

namespace H13y.Tests;

public class TryParseOrdinalTests
{
    [Theory]
    [InlineData("1st", 1)]
    [InlineData("2nd", 2)]
    [InlineData("3rd", 3)]
    [InlineData("4th", 4)]
    [InlineData("10th", 10)]
    [InlineData("11th", 11)]
    [InlineData("12th", 12)]
    [InlineData("13th", 13)]
    [InlineData("21st", 21)]
    [InlineData("22nd", 22)]
    [InlineData("103rd", 103)]
    [InlineData("111th", 111)]
    [InlineData("1000th", 1000)]
    public void Valid_ordinals_parse(string input, long expected)
    {
        Assert.True(H.TryParseOrdinal(input, out var value));
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("1th")]
    [InlineData("2st")]
    [InlineData("3nd")]
    [InlineData("11st")]
    [InlineData("21th")]
    [InlineData("103th")]
    [InlineData("st")]
    [InlineData("abc")]
    [InlineData("")]
    public void Invalid_ordinals_return_false(string input) =>
        Assert.False(H.TryParseOrdinal(input, out _));

    [Fact]
    public void Null_returns_false() => Assert.False(H.TryParseOrdinal(null!, out _));

    [Fact]
    public void Round_trip_small_set()
    {
        foreach (var n in new long[] { 1, 2, 3, 4, 5, 11, 12, 13, 21, 100, 101, 1000 })
        {
            var text = H.Ordinal(n);
            Assert.True(H.TryParseOrdinal(text, out var parsed));
            Assert.Equal(n, parsed);
        }
    }
}

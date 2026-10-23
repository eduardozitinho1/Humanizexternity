using Xunit;

namespace H13y.Tests;

public class WordTests
{
    [Theory]
    [InlineData(0, "zero")]
    [InlineData(1, "one")]
    [InlineData(9, "nine")]
    [InlineData(10, "ten")]
    [InlineData(11, "eleven")]
    [InlineData(15, "fifteen")]
    [InlineData(19, "nineteen")]
    [InlineData(20, "twenty")]
    [InlineData(21, "twenty-one")]
    [InlineData(22, "twenty-two")]
    [InlineData(99, "ninety-nine")]
    [InlineData(100, "one hundred")]
    [InlineData(101, "one hundred one")]
    [InlineData(110, "one hundred ten")]
    [InlineData(120, "one hundred twenty")]
    [InlineData(121, "one hundred twenty-one")]
    [InlineData(999, "nine hundred ninety-nine")]
    [InlineData(1000, "one thousand")]
    [InlineData(1001, "one thousand one")]
    [InlineData(1234, "one thousand two hundred thirty-four")]
    [InlineData(1_000_000, "one million")]
    [InlineData(1_000_000_000, "one billion")]
    [InlineData(1_000_000_000_000, "one trillion")]
    public void Word_formats_cardinal_number(long value, string expected) =>
        Assert.Equal(expected, H.Word(value));

    [Theory]
    [InlineData(-1, "minus one")]
    [InlineData(-15, "minus fifteen")]
    [InlineData(-1234, "minus one thousand two hundred thirty-four")]
    public void Word_formats_negative_number(long value, string expected) =>
        Assert.Equal(expected, H.Word(value));

    [Fact]
    public void Word_supports_MaxWordValue() =>
        Assert.Equal(
            "nine hundred ninety-nine trillion nine hundred ninety-nine billion "
                + "nine hundred ninety-nine million nine hundred ninety-nine thousand "
                + "nine hundred ninety-nine",
            H.Word(H.MaxWordValue)
        );

    [Fact]
    public void Word_supports_MinWordValue() =>
        Assert.Equal(
            "minus nine hundred ninety-nine trillion nine hundred ninety-nine billion "
                + "nine hundred ninety-nine million nine hundred ninety-nine thousand "
                + "nine hundred ninety-nine",
            H.Word(H.MinWordValue)
        );

    [Fact]
    public void Word_throws_above_MaxWordValue() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(H.MaxWordValue + 1));

    [Fact]
    public void Word_throws_below_MinWordValue() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(H.MinWordValue - 1));

    [Theory]
    [InlineData("zero", 0L)]
    [InlineData("one", 1L)]
    [InlineData("twenty", 20L)]
    [InlineData("twenty-two", 22L)]
    [InlineData("twenty two", 22L)]
    [InlineData("one hundred", 100L)]
    [InlineData("one hundred one", 101L)]
    [InlineData("one hundred and one", 101L)]
    [InlineData("one hundred twenty-two", 122L)]
    [InlineData("one hundred and twenty-two", 122L)]
    [InlineData("one thousand", 1000L)]
    [InlineData("one thousand two hundred thirty-four", 1234L)]
    [InlineData("two million", 2_000_000L)]
    [InlineData("minus fifteen", -15L)]
    [InlineData("negative fifteen", -15L)]
    public void TryParseWord_parses_valid_input(string text, long expected)
    {
        Assert.True(H.TryParseWord(text, out var value));
        Assert.Equal(expected, value);
    }

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData("hundred")]
[InlineData("one zero")]
[InlineData("zero one")]
[InlineData("twenty-zero")]
[InlineData("xxx")]
[InlineData("minus")]
[InlineData("and")]
[InlineData("and one")]
[InlineData("one and and two")]
[InlineData("twenty-two thousand and")]
public void TryParseWord_rejects_invalid_input(string text) =>
    Assert.False(H.TryParseWord(text, out _));

    [Fact]
    public void TryParseWord_rejects_null() => Assert.False(H.TryParseWord(null!, out _));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(22)]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(999)]
    [InlineData(1234)]
    [InlineData(1_000_000)]
    [InlineData(999_999_999_999_999)]
    [InlineData(-15)]
    public void Word_round_trips_through_TryParseWord(long value)
    {
        var text = H.Word(value);
        Assert.True(H.TryParseWord(text, out var parsed));
        Assert.Equal(value, parsed);
    }
}

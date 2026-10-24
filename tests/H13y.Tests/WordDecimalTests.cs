using Xunit;

namespace H13y.Tests;

public class WordDecimalTests
{
    [Theory]
    [InlineData(0, "zero")]
    [InlineData(1, "one")]
    [InlineData(22, "twenty-two")]
    [InlineData(100, "one hundred")]
    [InlineData(1234, "one thousand two hundred thirty-four")]
    public void Word_decimal_handles_whole_numbers(int value, string expected) =>
        Assert.Equal(expected, H.Word((decimal)value));

    [Theory]
    [InlineData("0.5", "zero point five")]
    [InlineData("22.5", "twenty-two point five")]
    [InlineData("100.001", "one hundred point zero zero one")]
    [InlineData("3.14", "three point one four")]
    [InlineData("0.05", "zero point zero five")]
    [InlineData("1000000.5", "one million point five")]
    public void Word_decimal_formats_fractional_part(string raw, string expected) =>
        Assert.Equal(expected, H.Word(decimal.Parse(raw, System.Globalization.CultureInfo.InvariantCulture)));

    [Theory]
    [InlineData("-0.5", "minus zero point five")]
    [InlineData("-22.5", "minus twenty-two point five")]
    [InlineData("-1000.001", "minus one thousand point zero zero one")]
    public void Word_decimal_formats_negative_numbers(string raw, string expected) =>
        Assert.Equal(expected, H.Word(decimal.Parse(raw, System.Globalization.CultureInfo.InvariantCulture)));

    [Fact]
    public void Word_decimal_trims_trailing_zeros() =>
        Assert.Equal("twenty-two point five", H.Word(22.50m));

    [Fact]
    public void Word_decimal_returns_word_for_22_point_0() =>
        Assert.Equal("twenty-two", H.Word(22.0m));

    [Fact]
    public void Word_double_default_max_decimals_removes_float_artifact() =>
        Assert.Equal("zero point three", H.Word(0.1 + 0.2));

    [Fact]
    public void Word_double_formats_simple_decimal() =>
        Assert.Equal("twenty-two point five", H.Word(22.5));

    [Fact]
    public void Word_double_respects_max_decimals() =>
        Assert.Equal("three point one", H.Word(3.14159, 1));

    [Fact]
    public void Word_double_throws_for_NaN() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(double.NaN));

    [Fact]
    public void Word_double_throws_for_infinity() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(double.PositiveInfinity));

    [Fact]
    public void Word_double_throws_for_out_of_range_value() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(1e30));

    [Fact]
    public void Word_double_throws_for_invalid_max_decimals() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word(3.14, -1));

    [Fact]
    public void Word_decimal_throws_above_max() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word((decimal)H.MaxWordValue + 1m));

    [Fact]
    public void Word_decimal_throws_below_min() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Word((decimal)H.MinWordValue - 1m));

    [Theory]
    [InlineData("zero", 0)]
    [InlineData("twenty-two", 22)]
    [InlineData("zero point five", 0.5)]
    [InlineData("twenty-two point five", 22.5)]
    [InlineData("three point one four", 3.14)]
    [InlineData("minus twenty-two point five", -22.5)]
    [InlineData("one hundred point zero zero one", 100.001)]
    public void TryParseDecimalWord_parses_valid_input(string text, double expected)
    {
        Assert.True(H.TryParseDecimalWord(text, out var value));
        Assert.Equal((decimal)expected, value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("point five")]
    [InlineData("twenty-two point")]
    [InlineData("twenty-two point fourteen")]
    [InlineData("twenty-two point ten")]
    [InlineData("point")]
    [InlineData("minus")]
    public void TryParseDecimalWord_rejects_invalid_input(string text) =>
        Assert.False(H.TryParseDecimalWord(text, out _));

    [Fact]
    public void TryParseDecimalWord_rejects_null() =>
        Assert.False(H.TryParseDecimalWord(null!, out _));

    [Theory]
    [InlineData("0")]
    [InlineData("22")]
    [InlineData("0.5")]
    [InlineData("22.5")]
    [InlineData("3.14")]
    [InlineData("-0.5")]
    [InlineData("-22.5")]
    [InlineData("100.001")]
    [InlineData("1000000.5")]
    public void Word_decimal_round_trips_through_TryParseDecimalWord(string raw)
    {
        var value = decimal.Parse(raw, System.Globalization.CultureInfo.InvariantCulture);
        var text = H.Word(value);
        Assert.True(H.TryParseDecimalWord(text, out var parsed));
        Assert.Equal(value, parsed);
    }

    [Fact]
    public void Word_decimal_round_trips_negative()
    {
        var text = H.Word(-22.5m);
        Assert.True(H.TryParseDecimalWord(text, out var parsed));
        Assert.Equal(-22.5m, parsed);
    }
}

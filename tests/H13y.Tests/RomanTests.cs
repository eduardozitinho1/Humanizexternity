using Xunit;

namespace H13y.Tests;

public class RomanTests
{
    [Theory]
    [InlineData(1, "I")]
    [InlineData(2, "II")]
    [InlineData(4, "IV")]
    [InlineData(5, "V")]
    [InlineData(9, "IX")]
    [InlineData(10, "X")]
    [InlineData(40, "XL")]
    [InlineData(50, "L")]
    [InlineData(90, "XC")]
    [InlineData(100, "C")]
    [InlineData(400, "CD")]
    [InlineData(500, "D")]
    [InlineData(900, "CM")]
    [InlineData(1000, "M")]
    [InlineData(1987, "MCMLXXXVII")]
    [InlineData(2024, "MMXXIV")]
    [InlineData(3999, "MMMCMXCIX")]
    public void ToRoman_subtractive(int value, string expected) =>
        Assert.Equal(expected, H.Roman(value));

    [Theory]
    [InlineData(4, "IIII")]
    [InlineData(9, "VIIII")]
    [InlineData(2024, "MMXXIIII")]
    public void ToRoman_additive(int value, string expected) =>
        Assert.Equal(expected, H.Roman(value, RomanStyle.Additive));

    [Theory]
    [InlineData("I", 1)]
    [InlineData("IV", 4)]
    [InlineData("IX", 9)]
    [InlineData("MCMLXXXVII", 1987)]
    [InlineData("MMXXIV", 2024)]
    [InlineData("mmxxiv", 2024)]
    [InlineData("MMMCMXCIX", 3999)]
    public void ParseRoman_valid(string text, int expected) =>
        Assert.Equal(expected, H.ParseRoman(text));

    [Theory]
    [InlineData("")]
    [InlineData("XYZ")]
    [InlineData("IIII")]
    [InlineData("IC")]
    [InlineData("MMMM")]
    public void ParseRoman_rejects_invalid(string text) =>
        Assert.Throws<FormatException>(() => H.ParseRoman(text));

    [Fact]
    public void TryParseRoman_returns_false_for_invalid()
    {
        Assert.False(H.TryParseRoman("garbage", out var value));
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryParseRoman_success()
    {
        Assert.True(H.TryParseRoman("MMXXIV", out var value));
        Assert.Equal(2024, value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(4000)]
    public void ToRoman_throws_out_of_range(int value) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.Roman(value));

    [Fact]
    public void Round_trip_all_valid_values()
    {
        for (var i = 1; i <= 3999; i++)
        {
            var roman = H.Roman(i);
            Assert.True(H.TryParseRoman(roman, out var parsed));
            Assert.Equal(i, parsed);
        }
    }
}

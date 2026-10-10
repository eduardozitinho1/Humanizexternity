using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class OrdinalTests
{
    [Theory]
    [InlineData(1, "1st")]
    [InlineData(2, "2nd")]
    [InlineData(3, "3rd")]
    [InlineData(4, "4th")]
    [InlineData(10, "10th")]
    [InlineData(11, "11th")]
    [InlineData(12, "12th")]
    [InlineData(13, "13th")]
    [InlineData(21, "21st")]
    [InlineData(22, "22nd")]
    [InlineData(23, "23rd")]
    [InlineData(103, "103rd")]
    [InlineData(111, "111th")]
    [InlineData(1000, "1000th")]
    public void Ordinal_suffix(long value, string expected) =>
        Assert.Equal(expected, H.Ordinal(value));

    [Theory]
    [InlineData(0, "zeroth")]
    [InlineData(1, "first")]
    [InlineData(2, "second")]
    [InlineData(3, "third")]
    [InlineData(5, "fifth")]
    [InlineData(9, "ninth")]
    [InlineData(10, "tenth")]
    [InlineData(11, "eleventh")]
    [InlineData(12, "twelfth")]
    [InlineData(20, "twentieth")]
    [InlineData(21, "twenty-first")]
    [InlineData(42, "forty-second")]
    [InlineData(100, "one hundredth")]
    [InlineData(101, "one hundred first")]
    [InlineData(121, "one hundred twenty-first")]
    [InlineData(999, "nine hundred ninety-ninth")]
    public void Ordinal_word(long value, string expected) =>
        Assert.Equal(expected, H.OrdinalWord(value));

    [Fact]
    public void OrdinalWord_throws_above_999() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.OrdinalWord(1000));

    [Fact]
    public void OrdinalWord_throws_for_negative() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => H.OrdinalWord(-1));

    [Fact]
    public void Ordinal_honors_culture()
    {
        var opts = new HumanizeOptions { Culture = new CultureInfo("pt-BR") };
        // pt-BR uses "." for thousands in some contexts, but "ToString()" without
        // formatting does not add separators, so "1" stays "1".
        Assert.Equal("1st", H.Ordinal(1, opts));
    }
}

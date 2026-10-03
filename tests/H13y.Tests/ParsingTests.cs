using Xunit;

namespace H13y.Tests;

public class ParsingTests
{
    [Theory]
    [InlineData("1 GB", 1, "GB")]
    [InlineData("1.5 KB", 1.5, "KB")]
    [InlineData("500 B", 500, "B")]
    [InlineData("1.5 kg", 1.5, "kg")]
    [InlineData("1500 g", 1500, "g")]
    public void Parse_returns_value_and_unit(string input, double value, string symbol)
    {
        var m = UnitParser.Parse(input);
        Assert.Equal(value, m.Value);
        Assert.Equal(symbol, m.Unit.Symbol);
    }

    [Fact]
    public void TryParse_returns_false_on_invalid()
    {
        Assert.False(UnitParser.TryParse("not a measure", out _));
    }

    [Fact]
    public void Parse_throws_on_unknown_unit()
    {
        Assert.Throws<FormatException>(() => UnitParser.Parse("1 xyz"));
    }
}

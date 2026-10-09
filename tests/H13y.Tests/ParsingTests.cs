using System.Globalization;
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
        var m = UnitParser.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(value, m.Value);
        Assert.Equal(symbol, m.Unit.Symbol);
    }

    [Fact]
    public void TryParse_returns_false_on_invalid()
    {
        Assert.False(UnitParser.TryParse("not a measure", CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void Parse_throws_on_unknown_unit()
    {
        Assert.Throws<FormatException>(() =>
            UnitParser.Parse("1 xyz", CultureInfo.InvariantCulture)
        );
    }

    [Theory]
    [InlineData("1:99")]
    [InlineData("1:30:99")]
    [InlineData("1:99:30")]
    public void TryParse_rejects_invalid_colon_durations(string input)
    {
        Assert.False(UnitParser.TryParse(input, CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void TryParse_returns_false_for_null_text()
    {
        Assert.False(UnitParser.TryParse(null!, CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void TryParse_returns_false_for_null_culture()
    {
        Assert.False(UnitParser.TryParse("1 GB", null!, out _));
    }

    [Theory]
    [InlineData("1.5e3 g", 1500, "g")]
    [InlineData("2E6 B", 2000000, "B")]
    [InlineData("2.5e-3 kg", 0.0025, "kg")]
    public void Parse_supports_scientific_notation(
        string input,
        double expectedValue,
        string expectedUnit
    )
    {
        var measure = UnitParser.Parse(input, CultureInfo.InvariantCulture);

        Assert.Equal(expectedValue, measure.Value);
        Assert.Equal(expectedUnit, measure.Unit.Symbol);
    }

    [Fact]
    public void Parse_supports_scientific_notation_with_comma_culture()
    {
        var culture = CultureInfo.GetCultureInfo("pt-BR");

        var measure = UnitParser.Parse("1,5e3 g", culture);

        Assert.Equal(1500, measure.Value);
        Assert.Equal("g", measure.Unit.Symbol);
    }
}

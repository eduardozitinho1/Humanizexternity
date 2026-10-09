using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class ParserNumericOverflowTests
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    [Fact]
    public void Parse_rejects_non_finite_scientific_notation()
    {
        Assert.Throws<FormatException>(() => UnitParser.Parse("1e999 g", Invariant));
    }

    [Fact]
    public void TryParse_rejects_non_finite_scientific_notation()
    {
        Assert.False(UnitParser.TryParse("1e999 g", Invariant, out _));
    }

    [Fact]
    public void TryParse_rejects_overflowing_compound_duration()
    {
        var huge = new string('9', 400);
        var input = $"{huge} h {huge} h";

        Assert.False(UnitParser.TryParse(input, Invariant, out _));
    }

    [Fact]
    public void TryParse_rejects_overflowing_colon_duration()
    {
        var huge = new string('9', 400);

        Assert.False(UnitParser.TryParse($"{huge}:00", Invariant, out _));
    }

    [Fact]
    public void TryParseDataSize_rejects_values_above_int64_range()
    {
        Assert.False(H.TryParseDataSize("1e20 B", Invariant, out var size));
        Assert.Equal(default, size);
    }

    [Fact]
    public void TypedTryParse_returns_false_for_null_text()
    {
        Assert.False(H.TryParseMass(null!, Invariant, out var mass));
        Assert.Equal(default, mass);
    }

    [Fact]
    public void TypedTryParse_returns_false_for_null_culture()
    {
        Assert.False(H.TryParseMass("1 g", null!, out var mass));
        Assert.Equal(default, mass);
    }
}

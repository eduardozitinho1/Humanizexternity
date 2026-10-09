using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void DataSize_zero_humanizes_as_zero_bytes()
    {
        Assert.Equal("0 B", DataSize.FromBytes(0).Humanize());
    }

    [Fact]
    public void Mass_zero_humanizes_as_zero_grams()
    {
        Assert.Equal("0 g", Mass.FromGrams(0).Humanize());
    }

    [Fact]
    public void Length_zero_humanizes_as_zero_meters()
    {
        Assert.Equal("0 m", Length.FromMeters(0).Humanize());
    }

    [Fact]
    public void Volume_zero_humanizes_as_zero_liters()
    {
        Assert.Equal("0 l", Volume.FromLiters(0).Humanize());
    }

    [Fact]
    public void Area_zero_humanizes_as_zero_square_meters()
    {
        Assert.Equal("0 m2", Area.FromSquareMeters(0).Humanize());
    }

    [Fact]
    public void DataSize_long_max_humanizes_without_overflow()
    {
        var result = DataSize.FromBytes(long.MaxValue).Humanize();
        Assert.Contains("PB", result);
    }

    [Fact]
    public void Mass_huge_value_humanizes_in_tonnes()
    {
        var result = Mass.FromTonnes(1_000_000).Humanize();
        Assert.Contains("t", result);
    }

    [Fact]
    public void Mass_milligram_precision()
    {
        Assert.Equal("1 mg", Mass.FromGrams(0.001).Humanize());
    }

    [Fact]
    public void Length_millimeter_precision()
    {
        Assert.Equal("1 mm", Length.FromMeters(0.001).Humanize());
    }

    [Fact]
    public void Mass_NaN_humanizes_as_NaN()
    {
        Assert.Equal("NaN", Mass.FromGrams(double.NaN).Humanize());
    }

    [Fact]
    public void Mass_positive_infinity_humanizes()
    {
        Assert.Equal("∞", Mass.FromGrams(double.PositiveInfinity).Humanize());
    }

    [Fact]
    public void Mass_negative_infinity_humanizes()
    {
        Assert.Equal("-∞", Mass.FromGrams(double.NegativeInfinity).Humanize());
    }

    [Fact]
    public void Duration_NaN_humanizes_as_NaN()
    {
        Assert.Equal("NaN", Duration.FromSeconds(double.NaN).Humanize());
    }

    [Fact]
    public void Duration_positive_infinity_humanizes()
    {
        Assert.Equal("∞", Duration.FromSeconds(double.PositiveInfinity).Humanize());
    }

    [Theory]
    [InlineData("1h30min", 5400)]
    [InlineData("1h 30min", 5400)]
    [InlineData("1 h 30 min", 5400)]
    [InlineData("1h30m", 5400)]
    [InlineData("1h30m45s", 5445)]
    [InlineData("30min", 1800)]
    [InlineData("45s", 45)]
    [InlineData("2d 12h", 216000)]
    public void Parse_compound_duration(string input, double expectedSeconds)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(expectedSeconds, m.ToBase(), precision: 6);
    }

    [Theory]
    [InlineData("1:30", 90)]
    [InlineData("0:30", 30)]
    [InlineData("1:30:45", 5445)]
    [InlineData("10:00", 600)]
    [InlineData("0:05", 5)]
    [InlineData("2:00:00", 7200)]
    public void Parse_colon_duration(string input, double expectedSeconds)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(expectedSeconds, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Parse_compound_with_unknown_unit_throws()
    {
        Assert.Throws<FormatException>(() => H.Parse("1h30xyz", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Parse_compound_with_non_time_unit_throws()
    {
        Assert.Throws<FormatException>(() => H.Parse("1kg2g", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Parse_compound_returns_seconds_unit()
    {
        var m = H.Parse("1h30min", CultureInfo.InvariantCulture);
        Assert.Equal("s", m.Unit.Symbol);
        Assert.Equal(Dimension.Time, m.Unit.Dimension);
    }

    [Fact]
    public void Parse_colon_returns_seconds_unit()
    {
        var m = H.Parse("1:30", CultureInfo.InvariantCulture);
        Assert.Equal("s", m.Unit.Symbol);
        Assert.Equal(Dimension.Time, m.Unit.Dimension);
    }
}

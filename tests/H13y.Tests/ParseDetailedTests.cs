using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class ParseDetailedTests
{
    [Fact]
    public void Valid_input_succeeds()
    {
        var r = H.ParseDetailed("1.5 kg", CultureInfo.InvariantCulture);
        Assert.True(r.Success);
        Assert.Equal(ParseErrorKind.None, r.ErrorKind);
        Assert.Equal(1.5, r.Measure.Value, precision: 6);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_input_reports_EmptyInput(string? text)
    {
        var r = H.ParseDetailed(text!, CultureInfo.InvariantCulture);
        Assert.False(r.Success);
        Assert.Equal(ParseErrorKind.EmptyInput, r.ErrorKind);
    }

    [Fact]
    public void Unknown_unit_reports_UnknownUnit()
    {
        var r = H.ParseDetailed("1 xyz", CultureInfo.InvariantCulture);
        Assert.False(r.Success);
        Assert.Equal(ParseErrorKind.UnknownUnit, r.ErrorKind);
    }

    [Fact]
    public void Malformed_number_reports_InvalidNumber()
    {
        var r = H.ParseDetailed("abc kg", CultureInfo.InvariantCulture);
        Assert.False(r.Success);
        Assert.Equal(ParseErrorKind.InvalidNumber, r.ErrorKind);
    }

    [Fact]
    public void Overflow_reports_Overflow()
    {
        var r = H.ParseDetailed("1e999 g", CultureInfo.InvariantCulture);
        Assert.False(r.Success);
        Assert.Equal(ParseErrorKind.Overflow, r.ErrorKind);
    }

    [Fact]
    public void Dimension_mismatch_is_reported()
    {
        var r = H.ParseDetailed("1.5 kg", Dimension.Data, CultureInfo.InvariantCulture);
        Assert.False(r.Success);
        Assert.Equal(ParseErrorKind.DimensionMismatch, r.ErrorKind);
    }

    [Fact]
    public void Dimension_match_succeeds()
    {
        var r = H.ParseDetailed("1.5 kg", Dimension.Mass, CultureInfo.InvariantCulture);
        Assert.True(r.Success);
    }

    [Fact]
    public void Error_message_is_populated()
    {
        var r = H.ParseDetailed("1 xyz", CultureInfo.InvariantCulture);
        Assert.NotNull(r.ErrorMessage);
        Assert.Contains("xyz", r.ErrorMessage);
    }

    [Fact]
    public void Compound_duration_is_supported()
    {
        var r = H.ParseDetailed("1h30min", CultureInfo.InvariantCulture);
        Assert.True(r.Success);
        Assert.Equal(5400, r.Measure.ToBase(), precision: 6);
    }

    [Fact]
    public void Colon_duration_is_supported()
    {
        var r = H.ParseDetailed("1:30:45", CultureInfo.InvariantCulture);
        Assert.True(r.Success);
        Assert.Equal(5445, r.Measure.ToBase(), precision: 6);
    }
}

using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class LengthTests
{
    [Fact]
    public void FromKilometers_converts_to_meters()
    {
        var length = Length.FromKilometers(1.5);
        Assert.Equal(1500, length.Meters);
    }

    [Theory]
    [InlineData(0.005, "5 mm")]
    [InlineData(0.5, "50 cm")]
    [InlineData(1.5, "1.5 m")]
    [InlineData(1500, "1.5 km")]
    public void Humanize_returns_correct_text(double meters, string expected)
    {
        var length = Length.FromMeters(meters);
        Assert.Equal(expected, length.Humanize());
    }
}

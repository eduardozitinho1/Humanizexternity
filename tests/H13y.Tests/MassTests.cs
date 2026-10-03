using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class MassTests
{
    [Fact]
    public void FromKilograms_converts_to_grams()
    {
        var mass = Mass.FromKilograms(1.5);
        Assert.Equal(1500, mass.Grams);
    }

    [Theory]
    [InlineData(500, "500 g")]
    [InlineData(1500, "1.5 kg")]
    [InlineData(1_500_000, "1.5 t")]
    public void Humanize_returns_correct_text(double grams, string expected)
    {
        var mass = Mass.FromGrams(grams);
        Assert.Equal(expected, mass.Humanize());
    }
}

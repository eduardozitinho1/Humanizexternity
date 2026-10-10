using Xunit;

namespace H13y.Tests;

public class UncertaintyTests
{
    [Fact]
    public void Uncertainty_mass() =>
        Assert.Equal("1.5 kg ± 50 g", H.Uncertainty(1500, 50, Dimension.Mass));

    [Fact]
    public void Uncertainty_length() =>
        Assert.Equal("1.5 m ± 5 cm", H.Uncertainty(1.5, 0.05, Dimension.Length));

    [Fact]
    public void Uncertainty_zero_uncertainty() =>
        Assert.Equal("1.5 kg ± 0 g", H.Uncertainty(1500, 0, Dimension.Mass));

    [Fact]
    public void Uncertainty_negative_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            H.Uncertainty(100, -1, Dimension.Mass)
        );

    [Fact]
    public void Uncertainty_temperature_throws() =>
        Assert.Throws<ArgumentException>(() =>
            H.Uncertainty(300, 5, Dimension.Temperature)
        );
}

using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class RangeDimensionTests
{
    [Fact]
    public void Range_with_dimension_humanizes_each_endpoint() =>
        Assert.Equal("1000 B–1.5 KB", H.Range(1000, 1500, Dimension.Data));

    [Fact]
    public void Range_with_dimension_mass() =>
        Assert.Equal("500 g–1.5 kg", H.Range(500, 1500, Dimension.Mass));

    [Fact]
    public void Range_with_dimension_and_to_style() =>
        Assert.Equal(
            "500 g to 1.5 kg",
            H.Range(500, 1500, Dimension.Mass, RangeStyle.To)
        );

    [Fact]
    public void Range_temperature_throws() =>
        Assert.Throws<ArgumentException>(() => H.Range(0, 100, Dimension.Temperature));

    [Fact]
    public void Range_honors_options()
    {
        var opts = new HumanizeOptions { MaxDecimals = 3, Culture = new CultureInfo("pt-BR") };
        Assert.Equal("500 g–1,5 kg", H.Range(500, 1500, Dimension.Mass, opts));
    }
}

using Xunit;

namespace H13y.Tests;

public class FormattingTests
{
    [Fact]
    public void H_Bytes_returns_humanized_string()
    {
        Assert.Equal("1 GB", H.Bytes(1073741824));
    }

    [Fact]
    public void H_Best_picks_best_unit()
    {
        Assert.Equal("1.5 kg", H.Best(1500, Dimension.Mass));
    }

    [Fact]
    public void Options_no_space()
    {
        var opts = new HumanizeOptions { SpaceBetweenValueAndUnit = false };
        Assert.Equal("1GB", H.Bytes(1073741824, opts));
    }

    [Fact]
    public void Options_max_decimals()
    {
        var opts = new HumanizeOptions { MaxDecimals = 3 };
        Assert.Equal("1.5 GB", H.Bytes(1610612736, opts));
    }
}

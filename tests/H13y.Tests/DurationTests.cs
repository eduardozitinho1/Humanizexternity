using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class DurationTests
{
    [Fact]
    public void FromHours_converts_to_seconds()
    {
        var d = Duration.FromHours(1.5);
        Assert.Equal(5400, d.Seconds);
    }

    [Theory]
    [InlineData(0.5, "500 ms")]
    [InlineData(30, "30 s")]
    [InlineData(90, "1 min 30 s")]
    [InlineData(3661, "1 h 1 min 1 s")]
    [InlineData(7200, "2 h")]
    [InlineData(90000, "1 d 1 h")]
    [InlineData(604800, "1 w")]
    public void Humanize_returns_correct_text(double seconds, string expected)
    {
        var d = Duration.FromSeconds(seconds);
        Assert.Equal(expected, d.Humanize());
    }

    [Fact]
    public void Humanize_handles_week_counts_above_int_max()
    {
        var seconds = ((double)int.MaxValue + 1) * 604800;
        var duration = Duration.FromSeconds(seconds);

        Assert.Equal("2147483648 w", duration.Humanize());
    }
}

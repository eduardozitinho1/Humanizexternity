using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class TryParseTimeSpanTests
{
    [Fact]
    public void TryParseTimeSpan_returns_false_when_duration_exceeds_timespan_range()
    {
        var success = H.TryParseTimeSpan("1e20 s", CultureInfo.InvariantCulture, out var span);

        Assert.False(success);
        Assert.Equal(TimeSpan.Zero, span);
    }

    [Fact]
    public void TryParseTimeSpan_returns_true_for_valid_duration()
    {
        var success = H.TryParseTimeSpan("90 s", CultureInfo.InvariantCulture, out var span);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromSeconds(90), span);
    }
}

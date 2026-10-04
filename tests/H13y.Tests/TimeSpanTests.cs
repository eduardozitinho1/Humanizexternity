using H13y.Measures;
using Xunit;
using System.Globalization;

namespace H13y.Tests;

public class TimeSpanTests
{
    [Fact]
    public void FromTimeSpan_preserves_total_seconds()
    {
        var span = TimeSpan.FromMilliseconds(1500);
        var d = Duration.FromTimeSpan(span);
        Assert.Equal(1.5, d.Seconds, precision: 6);
    }

    [Fact]
    public void ToTimeSpan_round_trips()
    {
        var original = TimeSpan.FromSeconds(3661.5);
        var d = Duration.FromTimeSpan(original);
        var restored = d.ToTimeSpan();
        Assert.Equal(original, restored);
    }

    [Fact]
    public void FromTimeSpan_zero()
    {
        var d = Duration.FromTimeSpan(TimeSpan.Zero);
        Assert.Equal(0, d.Seconds);
    }

    [Fact]
    public void FromTimeSpan_negative()
    {
        var span = TimeSpan.FromSeconds(-30);
        var d = Duration.FromTimeSpan(span);
        Assert.Equal(-30, d.Seconds, precision: 6);
    }

    [Fact]
    public void H_TimeSpan_humanizes()
    {
        var span = TimeSpan.FromSeconds(3661);
        Assert.Equal("1 h 1 min 1 s", H.TimeSpan(span));
    }

    [Fact]
    public void H_TimeSpan_sub_second()
    {
        var span = TimeSpan.FromMilliseconds(500);
        Assert.Equal("500 ms", H.TimeSpan(span));
    }

    [Fact]
    public void H_TimeSpan_with_options()
    {
        var span = TimeSpan.FromSeconds(90);
        var opts = new HumanizeOptions { SpaceBetweenValueAndUnit = false };
        Assert.Equal("1min30s", H.TimeSpan(span, opts));
    }

    [Fact]
    public void ParseTimeSpan_compound()
    {
        var span = H.ParseTimeSpan("1h30min", CultureInfo.InvariantCulture);
        Assert.Equal(TimeSpan.FromMinutes(90), span);
    }

    [Fact]
    public void ParseTimeSpan_colon()
    {
        var span = H.ParseTimeSpan("1:30:45", CultureInfo.InvariantCulture);
        Assert.Equal(new TimeSpan(1, 30, 45), span);
    }

    [Fact]
    public void ParseTimeSpan_simple()
    {
        var span = H.ParseTimeSpan("1500 ms", CultureInfo.InvariantCulture);
        Assert.Equal(TimeSpan.FromMilliseconds(1500), span);
    }

    [Fact]
    public void ParseTimeSpan_throws_for_non_time_dimension()
    {
        Assert.Throws<FormatException>(() => H.ParseTimeSpan("1.5 kg", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void TryParseTimeSpan_success()
    {
        Assert.True(H.TryParseTimeSpan("2h", CultureInfo.InvariantCulture, out var span));
        Assert.Equal(TimeSpan.FromHours(2), span);
    }

    [Fact]
    public void TryParseTimeSpan_failure_returns_false()
    {
        Assert.False(H.TryParseTimeSpan("garbage", CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void TryParseTimeSpan_rejects_non_time_dimension()
    {
        Assert.False(H.TryParseTimeSpan("1.5 kg", CultureInfo.InvariantCulture, out _));
    }

    [Fact]
    public void TimeSpan_days_weeks_round_trip()
    {
        var original = TimeSpan.FromDays(8);
        var d = Duration.FromTimeSpan(original);
        Assert.Equal(8.0 / 7.0, d.ToWeeks(), precision: 6);
        Assert.Equal(original, d.ToTimeSpan());
    }
}

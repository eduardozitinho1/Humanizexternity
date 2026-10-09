using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class RelativeTimeTests
{
    private static readonly DateTime Now = new(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Just_now_for_recent_past() =>
        Assert.Equal("just now", H.RelativeTime(Now.AddSeconds(-10), Now));

    [Fact]
    public void In_a_moment_for_recent_future() =>
        Assert.Equal("in a moment", H.RelativeTime(Now.AddSeconds(20), Now));

    [Theory]
    [InlineData(5, "just now")]
    [InlineData(30, "just now")]
    [InlineData(44, "just now")]
    public void Sub_minute_past_is_just_now(int secondsOffset, string expected) =>
        Assert.Equal(expected, H.RelativeTime(Now.AddSeconds(-secondsOffset), Now));

    [Theory]
    [InlineData(60, "1 minute ago")]
    [InlineData(90, "2 minutes ago")]
    [InlineData(120, "2 minutes ago")]
    [InlineData(1800, "30 minutes ago")]
    public void Past_minutes(int secondsOffset, string expected) =>
        Assert.Equal(expected, H.RelativeTime(Now.AddSeconds(-secondsOffset), Now));

    [Theory]
    [InlineData(1, "in 1 hour")]
    [InlineData(2, "in 2 hours")]
    [InlineData(5, "in 5 hours")]
    public void Future_hours(int hours, string expected) =>
        Assert.Equal(expected, H.RelativeTime(Now.AddHours(hours), Now));

    [Fact]
    public void Days_ago() => Assert.Equal("3 days ago", H.RelativeTime(Now.AddDays(-3), Now));

    [Fact]
    public void Weeks_ago() => Assert.Equal("2 weeks ago", H.RelativeTime(Now.AddDays(-14), Now));

    [Fact]
    public void Months_ago() => Assert.Equal("6 months ago", H.RelativeTime(Now.AddDays(-180), Now));

    [Fact]
    public void Years_ago() => Assert.Equal("2 years ago", H.RelativeTime(Now.AddDays(-730), Now));

    [Fact]
    public void Singular_year() => Assert.Equal("1 year ago", H.RelativeTime(Now.AddDays(-400), Now));

    [Fact]
    public void DateTimeOffset_overload()
    {
        var target = new DateTimeOffset(Now.AddMinutes(-5), TimeSpan.Zero);
        var reference = new DateTimeOffset(Now, TimeSpan.Zero);
        Assert.Equal("5 minutes ago", H.RelativeTime(target, reference));
    }

    [Fact]
    public void Symbol_style_when_explicitly_requested()
    {
        var opts = new HumanizeOptions { UnitStyle = UnitStyle.Symbol };
        Assert.Equal("5 min ago", H.RelativeTime(Now.AddMinutes(-5), Now, opts));
    }

    [Fact]
    public void Culture_is_honored_in_future_phrase()
    {
        var opts = new HumanizeOptions
        {
            UnitStyle = UnitStyle.FullName,
            Culture = new CultureInfo("pt-BR"),
        };
        Assert.Equal("in 3 hours", H.RelativeTime(Now.AddHours(3), Now, opts));
    }
}

using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class DownloadTimeTests
{
    [Fact]
    public void Duration_from_bytes_and_bits_per_second()
    {
        // 1 MB (1_000_000 bytes) at 8_000_000 bps = 1 second
        var d = H.DownloadDuration(bytes: 1_000_000, bitsPerSecond: 8_000_000);
        Assert.Equal(1.0, d.Seconds, precision: 6);
    }

    [Fact]
    public void Duration_from_data_size_and_bits_rate()
    {
        var size = DataSize.FromBytes(12_500_000);
        var rate = Bits.FromMegabits(100);
        var d = H.DownloadDuration(size, rate);
        Assert.Equal(1.0, d.Seconds, precision: 6);
    }

    [Fact]
    public void Download_time_is_humanized()
    {
        // 45_000_000 bytes at 100 Mbps = 3.6 seconds
        var text = H.DownloadTime(bytes: 45_000_000, bitsPerSecond: 100_000_000);
        Assert.Equal("3.6 s", text);
    }

    [Fact]
    public void Download_time_in_minutes()
    {
        // 5 GB at 100 Mbps ~ 7 minutes
        var size = DataSize.FromBytes(5_000_000_000);
        var rate = Bits.FromMegabits(100);
        var text = H.DownloadTime(size, rate);
        Assert.StartsWith("6", text);
        Assert.Contains("min", text);
    }

    [Fact]
    public void Zero_size_gives_zero_duration()
    {
        var d = H.DownloadDuration(bytes: 0, bitsPerSecond: 1_000_000);
        Assert.Equal(0, d.Seconds);
    }

    [Fact]
    public void Negative_bytes_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            H.DownloadDuration(bytes: -1, bitsPerSecond: 1_000_000)
        );

    [Fact]
    public void Zero_rate_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            H.DownloadDuration(bytes: 100, bitsPerSecond: 0)
        );

    [Fact]
    public void Negative_rate_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            H.DownloadDuration(bytes: 100, bitsPerSecond: -1)
        );
}

using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class DataSizeTests
{
    [Fact]
    public void FromKilobytes_converts_to_bytes()
    {
        var size = DataSize.FromKilobytes(1.5);
        Assert.Equal(1536, size.Bytes);
    }

    [Theory]
    [InlineData(500L, "500 B")]
    [InlineData(1024L, "1 KB")]
    [InlineData(1536L, "1.5 KB")]
    [InlineData(1073741824L, "1 GB")]
    public void Humanize_returns_correct_text(long bytes, string expected)
    {
        var size = DataSize.FromBytes(bytes);
        Assert.Equal(expected, size.Humanize());
    }
}

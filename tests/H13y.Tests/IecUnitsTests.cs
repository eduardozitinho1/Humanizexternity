using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class IecUnitsTests
{
    [Fact]
    public void Default_output_uses_traditional_symbols()
    {
        Assert.Equal("1 KB", DataSize.FromKilobytes(1).Humanize());
        Assert.Equal("1 MB", DataSize.FromMegabytes(1).Humanize());
        Assert.Equal("1 GB", DataSize.FromGigabytes(1).Humanize());
    }

    [Fact]
    public void Iec_option_uses_iec_symbols()
    {
        var opts = new HumanizeOptions { UseIecSymbols = true };
        Assert.Equal("1 KiB", DataSize.FromKilobytes(1).Humanize(opts));
        Assert.Equal("1 MiB", DataSize.FromMegabytes(1).Humanize(opts));
        Assert.Equal("1 GiB", DataSize.FromGigabytes(1).Humanize(opts));
    }

    [Fact]
    public void Iec_option_does_not_change_bytes()
    {
        var opts = new HumanizeOptions { UseIecSymbols = true };
        Assert.Equal("500 B", DataSize.FromBytes(500).Humanize(opts));
    }

    [Fact]
    public void Numeric_factor_is_unchanged_between_styles()
    {
        var traditional = DataSize.FromKilobytes(1).Humanize();
        var iec = DataSize.FromKilobytes(1).Humanize(new HumanizeOptions { UseIecSymbols = true });
        Assert.Equal("1 KB", traditional);
        Assert.Equal("1 KiB", iec);
    }

    [Theory]
    [InlineData("1 KiB", 1024)]
    [InlineData("1 MiB", 1024 * 1024)]
    [InlineData("1 GiB", 1024L * 1024 * 1024)]
    [InlineData("1 kib", 1024)]
    [InlineData("1 kibibyte", 1024)]
    [InlineData("1 mebibytes", 1024 * 1024)]
    public void Parser_accepts_iec_symbols_and_names(string input, long expectedBytes)
    {
        var m = H.Parse(input);
        Assert.Equal(expectedBytes, (long)m.ToBase());
    }

    [Fact]
    public void Non_data_dimensions_ignore_iec_option()
    {
        var opts = new HumanizeOptions { UseIecSymbols = true };
        Assert.Equal("1.5 kg", Mass.FromGrams(1500).Humanize(opts));
        Assert.Equal("1.5 km", Length.FromMeters(1500).Humanize(opts));
    }
}

using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class FormattableTests
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    [Fact]
    public void DataSize_default_humanizes()
    {
        var size = DataSize.FromKilobytes(1536);
        Assert.Equal("1.5 MB", size.ToString());
    }

    [Fact]
    public void DataSize_H_format_humanizes()
    {
        var size = DataSize.FromKilobytes(1536);
        Assert.Equal("1.5 MB", size.ToString("H", Invariant));
    }

    [Fact]
    public void DataSize_I_format_uses_iec_symbols()
    {
        var size = DataSize.FromKilobytes(1536);
        Assert.Equal("1.5 MiB", size.ToString("I", Invariant));
    }

    [Fact]
    public void DataSize_R_format_returns_raw_bytes()
    {
        var size = DataSize.FromKilobytes(1);
        Assert.Equal("1024", size.ToString("R", Invariant));
    }

    [Fact]
    public void DataSize_numeric_format_drops_symbol()
    {
        var size = DataSize.FromKilobytes(1536);
        Assert.Equal("1.50", size.ToString("N2", Invariant));
    }

    [Fact]
    public void Mass_default_humanizes()
    {
        var mass = Mass.FromGrams(1500);
        Assert.Equal("1.5 kg", mass.ToString());
    }

    [Fact]
    public void Mass_R_format_returns_raw_grams()
    {
        var mass = Mass.FromKilograms(2);
        Assert.Equal("2000", mass.ToString("R", Invariant));
    }

    [Fact]
    public void Length_default_humanizes()
    {
        var length = Length.FromMeters(1500);
        Assert.Equal("1.5 km", length.ToString());
    }

    [Fact]
    public void Duration_default_humanizes()
    {
        var d = Duration.FromSeconds(3661);
        Assert.Equal("1 h 1 min 1 s", d.ToString());
    }

    [Fact]
    public void Duration_R_format_returns_raw_seconds()
    {
        var d = Duration.FromSeconds(90);
        Assert.Equal("90", d.ToString("R", Invariant));
    }

    [Fact]
    public void Volume_default_humanizes()
    {
        var v = Volume.FromLiters(1.5);
        Assert.Equal("1.5 l", v.ToString());
    }

    [Fact]
    public void Area_default_humanizes()
    {
        var a = Area.FromHectares(1);
        Assert.Equal("1 ha", a.ToString());
    }

    [Fact]
    public void Temperature_default_humanizes_in_celsius()
    {
        var t = Temperature.FromCelsius(25);
        Assert.Equal("25 °C", t.ToString());
    }

    [Fact]
    public void Temperature_HF_format_uses_fahrenheit()
    {
        var t = Temperature.FromCelsius(25);
        Assert.Equal("77 °F", t.ToString("H:F", Invariant));
    }

    [Fact]
    public void Temperature_HK_format_uses_kelvin()
    {
        var t = Temperature.FromCelsius(0);
        var s = t.ToString("H:K", Invariant);
        Assert.StartsWith("273", s);
        Assert.EndsWith("K", s);
    }

    [Fact]
    public void Temperature_R_format_returns_raw_kelvin()
    {
        var t = Temperature.FromCelsius(0);
        Assert.Equal("273.15", t.ToString("R", Invariant));
    }

    [Fact]
    public void Temperature_unknown_scale_throws()
    {
        var t = Temperature.FromCelsius(0);
        Assert.Throws<FormatException>(() => t.ToString("H:X", Invariant));
    }

    [Fact]
    public void Culture_is_honored()
    {
        var size = DataSize.FromKilobytes(1536);
        var ptBr = new CultureInfo("pt-BR");
        Assert.Equal("1,5 MB", size.ToString("H", ptBr));
    }

    [Fact]
    public void All_measures_implement_IFormattable()
    {
        Assert.IsAssignableFrom<IFormattable>(DataSize.FromBytes(1024));
        Assert.IsAssignableFrom<IFormattable>(Mass.FromGrams(1));
        Assert.IsAssignableFrom<IFormattable>(Length.FromMeters(1));
        Assert.IsAssignableFrom<IFormattable>(Duration.FromSeconds(1));
        Assert.IsAssignableFrom<IFormattable>(Volume.FromLiters(1));
        Assert.IsAssignableFrom<IFormattable>(Area.FromSquareMeters(1));
        Assert.IsAssignableFrom<IFormattable>(Temperature.FromKelvin(1));
    }
}

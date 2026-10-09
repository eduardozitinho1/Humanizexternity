using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class UnitStyleTests
{
    private static readonly HumanizeOptions Full = new() { UnitStyle = UnitStyle.FullName };

    [Fact]
    public void Default_style_is_symbol()
    {
        Assert.Equal(UnitStyle.Symbol, HumanizeOptions.Default.UnitStyle);
        Assert.Equal("1.5 kg", Mass.FromGrams(1500).Humanize());
    }

    [Theory]
    [InlineData(1, "1 gram")]
    [InlineData(2, "2 grams")]
    [InlineData(500, "500 grams")]
    [InlineData(1000, "1 kilogram")]
    [InlineData(1500, "1.5 kilograms")]
    public void Full_name_pluralizes_mass(double grams, string expected) =>
        Assert.Equal(expected, Mass.FromGrams(grams).Humanize(Full));

    [Fact]
    public void Full_name_applies_to_data_size()
    {
        Assert.Equal("1 gigabyte", DataSize.FromGigabytes(1).Humanize(Full));
        Assert.Equal("2 gigabytes", DataSize.FromGigabytes(2).Humanize(Full));
        Assert.Equal("1 byte", DataSize.FromBytes(1).Humanize(Full));
    }

    [Fact]
    public void Full_name_honors_iec_symbols()
    {
        var opts = Full with { UseIecSymbols = true };
        Assert.Equal("1 kibibyte", DataSize.FromKilobytes(1).Humanize(opts));
    }

    [Fact]
    public void Full_name_for_duration_uses_compound_parts() =>
        Assert.Equal("1 hour 1 minute 1 second", Duration.FromSeconds(3661).Humanize(Full));

    [Fact]
    public void Full_name_for_temperature()
    {
        Assert.Equal("25 degrees Celsius", Temperature.FromCelsius(25).Humanize(options: Full));
        Assert.Equal("1 degree Celsius", Temperature.FromCelsius(1).Humanize(options: Full));
        Assert.Equal(
            "77 degrees Fahrenheit",
            Temperature.FromFahrenheit(77).Humanize(TemperatureScale.Fahrenheit, Full)
        );
    }

    [Fact]
    public void Full_name_for_length()
    {
        Assert.Equal("1.5 kilometers", Length.FromMeters(1500).Humanize(Full));
        Assert.Equal("1 kilometer", Length.FromMeters(1000).Humanize(Full));
    }

    [Fact]
    public void Pluralize_false_forces_singular()
    {
        var opts = Full with { Pluralize = false };
        Assert.Equal("2 gram", Mass.FromGrams(2).Humanize(opts));
    }

    [Fact]
    public void No_space_option_still_works_with_full_names()
    {
        var opts = Full with { SpaceBetweenValueAndUnit = false };
        Assert.Equal("1kilogram", Mass.FromKilograms(1).Humanize(opts));
    }

    [Fact]
    public void Culture_affects_decimal_separator_with_full_names()
    {
        var opts = Full with { Culture = new CultureInfo("pt-BR") };
        Assert.Equal("1,5 kilograms", Mass.FromGrams(1500).Humanize(opts));
    }

    [Fact]
    public void H_facade_honors_full_name_style()
    {
        Assert.Equal("1.5 kilograms", H.Grams(1500, Full));
        Assert.Equal("1 gigabyte", H.Bytes(1073741824, Full));
        Assert.Equal("100 kilometers per hour", H.KilometersPerHour(100, Full));
    }
}

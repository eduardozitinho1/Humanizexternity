using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

/// <summary>
/// Every string the humanizer emits under <see cref="UnitStyle.FullName"/> must
/// parse back to the same base value. These tests iterate the registered units
/// so future additions are covered automatically.
/// </summary>
public class FullNameRoundTripTests
{
    private static readonly HumanizeOptions Full = new()
    {
        UnitStyle = UnitStyle.FullName,
        MaxDecimals = 2,
    };
    private static readonly HumanizeOptions FullIec = Full with { UseIecSymbols = true };

    public static IEnumerable<object[]> AutoSelectedUnits()
    {
        foreach (var unit in Units.All)
        {
            // Temperature is affine and uses TemperatureFormatter instead of Best.
            // Time uses compound notation handled by FormatDuration.
            if (unit.Dimension is Dimension.Temperature or Dimension.Time)
                continue;

            yield return new object[] { unit };
        }
    }

    [Theory]
    [MemberData(nameof(AutoSelectedUnits))]
    public void Singular_round_trips(Unit unit)
    {
        var baseValue = unit.Factor;
        var text = H.Best(baseValue, unit.Dimension, Full);
        var parsed = H.Parse(text, CultureInfo.InvariantCulture);

        Assert.Equal(unit.Dimension, parsed.Unit.Dimension);
        Assert.Equal(baseValue, parsed.ToBase(), precision: 6);
    }

    [Theory]
    [MemberData(nameof(AutoSelectedUnits))]
    public void Plural_round_trips(Unit unit)
    {
        var baseValue = unit.Factor * 2;
        var text = H.Best(baseValue, unit.Dimension, Full);
        var parsed = H.Parse(text, CultureInfo.InvariantCulture);

        Assert.Equal(unit.Dimension, parsed.Unit.Dimension);
        Assert.Equal(baseValue, parsed.ToBase(), precision: 6);
    }

    [Fact]
    public void Data_size_iec_singular_round_trips()
    {
        var text = DataSize.FromBytes(1024).Humanize(FullIec);
        Assert.Equal("1 kibibyte", text);

        var parsed = H.Parse(text, CultureInfo.InvariantCulture);
        Assert.Equal(1024, (long)parsed.ToBase());
    }

    [Fact]
    public void Data_size_iec_plural_round_trips()
    {
        var text = DataSize.FromBytes(2048).Humanize(FullIec);
        Assert.Equal("2 kibibytes", text);

        var parsed = H.Parse(text, CultureInfo.InvariantCulture);
        Assert.Equal(2048, (long)parsed.ToBase());
    }

    [Theory]
    [InlineData(TemperatureScale.Celsius)]
    [InlineData(TemperatureScale.Fahrenheit)]
    [InlineData(TemperatureScale.Kelvin)]
    public void Temperature_round_trips(TemperatureScale scale)
    {
        var t = Temperature.FromCelsius(25);
        var text = t.Humanize(scale, Full);

        var parsed = H.Parse(text, CultureInfo.InvariantCulture);

        Assert.Equal(Dimension.Temperature, parsed.Unit.Dimension);
        Assert.Equal(t.Kelvin, parsed.ToBase(), precision: 2);
    }

    [Fact]
    public void Temperature_singular_round_trips()
    {
        var t = Temperature.FromCelsius(1);
        var text = t.Humanize(TemperatureScale.Celsius, Full);
        Assert.Equal("1 degree Celsius", text);

        var parsed = H.Parse(text, CultureInfo.InvariantCulture);
        Assert.Equal(t.Kelvin, parsed.ToBase(), precision: 2);
    }

    [Fact]
    public void Compound_duration_round_trips()
    {
        var d = Duration.FromSeconds(3661);
        var text = d.Humanize(Full);
        Assert.Equal("1 hour 1 minute 1 second", text);

        var parsed = H.Parse(text, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Time, parsed.Unit.Dimension);
        Assert.Equal(d.Seconds, parsed.ToBase(), precision: 6);
    }
}

using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class SpeedTests
{
    [Fact]
    public void FromKilometersPerHour_to_mps() =>
        Assert.Equal(10, Speed.FromKilometersPerHour(36).MetersPerSecond, precision: 6);

    [Fact]
    public void FromMilesPerHour_to_mps() =>
        Assert.Equal(26.8224, Speed.FromMilesPerHour(60).MetersPerSecond, precision: 3);

    [Fact]
    public void FromKnots_to_mps() =>
        Assert.Equal(0.514444, Speed.FromKnots(1).MetersPerSecond, precision: 5);

    [Fact]
    public void ToKilometersPerHour_round_trips() =>
        Assert.Equal(36, Speed.FromMetersPerSecond(10).ToKilometersPerHour(), precision: 6);

    [Theory]
    [InlineData(100, "100 km/h")]
    [InlineData(1, "1 km/h")]
    [InlineData(0.5, "0.5 km/h")]
    public void Humanize_kph(double kph, string expected) =>
        Assert.Equal(expected, Speed.FromKilometersPerHour(kph).Humanize());

    [Fact]
    public void Humanize_mph_renders_as_kmh() =>
        Assert.Equal("96.6 km/h", Speed.FromMilesPerHour(60).Humanize());

    [Fact]
    public void Zero_humanizes_in_kmh() =>
        Assert.Equal("0 km/h", Speed.FromMetersPerSecond(0).Humanize());

    [Theory]
    [InlineData("100 km/h", 100)]
    [InlineData("100km/h", 100)]
    [InlineData("100 kmh", 100)]
    [InlineData("100 kph", 100)]
    [InlineData("60 mph", 60)]
    [InlineData("20 kn", 20)]
    [InlineData("20 kt", 20)]
    [InlineData("10 m/s", 10)]
    [InlineData("10 mps", 10)]
    [InlineData("32 ft/s", 32)]
    public void Parse_variants(string input, double expected)
    {
        var m = H.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Speed, m.Unit.Dimension);
        Assert.Equal(expected, m.Value, precision: 3);
    }

    [Fact]
    public void H_KilometersPerHour() => Assert.Equal("100 km/h", H.KilometersPerHour(100));

    [Fact]
    public void H_MilesPerHour() => Assert.Equal("96.6 km/h", H.MilesPerHour(60));

    [Fact]
    public void Conversion_round_trip_kph_mps()
    {
        var mps = H.Convert(90, Units.Speed.KilometerPerHour, Units.Speed.MeterPerSecond);
        var back = H.Convert(mps, Units.Speed.MeterPerSecond, Units.Speed.KilometerPerHour);
        Assert.Equal(90, back, precision: 6);
    }

    [Fact]
    public void Conversion_kph_to_mph() =>
        Assert.Equal(
            62.137,
            H.Convert(100, Units.Speed.KilometerPerHour, Units.Speed.MilePerHour),
            precision: 3
        );

    [Fact]
    public void Comparison_orders_by_mps()
    {
        Assert.True(Speed.FromMilesPerHour(60) > Speed.FromKilometersPerHour(30));
    }

    [Fact]
    public void Equality_across_units() =>
        Assert.Equal(Speed.FromMetersPerSecond(10), Speed.FromKilometersPerHour(36));
}

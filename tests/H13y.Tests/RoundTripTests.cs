using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class RoundTripTests
{
    // ===== Mass =====

    [Theory]
    [InlineData(500)]
    [InlineData(1500)]
    [InlineData(1_500_000)]
    [InlineData(0.5)]
    public void Mass_humanize_then_parse_preserves_value(double grams)
    {
        var original = Mass.FromGrams(grams);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Mass, parsed.Unit.Dimension);
        Assert.Equal(original.Grams, parsed.ToBase(), precision: 4);
    }

    // ===== Length =====

    [Theory]
    [InlineData(0.005)]
    [InlineData(0.5)]
    [InlineData(1.5)]
    [InlineData(1500)]
    public void Length_humanize_then_parse_preserves_value(double meters)
    {
        var original = Length.FromMeters(meters);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Length, parsed.Unit.Dimension);
        Assert.Equal(original.Meters, parsed.ToBase(), precision: 4);
    }

    // ===== Volume =====

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.5)]
    [InlineData(1500)]
    public void Volume_humanize_then_parse_preserves_value(double liters)
    {
        var original = Volume.FromLiters(liters);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Volume, parsed.Unit.Dimension);
        Assert.Equal(original.Liters, parsed.ToBase(), precision: 4);
    }

    // ===== Area =====

    [Theory]
    [InlineData(100)]
    [InlineData(10_000)]
    [InlineData(1_000_000)]
    public void Area_humanize_then_parse_preserves_value(double squareMeters)
    {
        var original = Area.FromSquareMeters(squareMeters);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Area, parsed.Unit.Dimension);
        Assert.Equal(original.SquareMeters, parsed.ToBase(), precision: 4);
    }

    // ===== Duration =====

    [Theory]
    [InlineData(45)]
    [InlineData(90)]
    [InlineData(3661)]
    [InlineData(90000)]
    public void Duration_humanize_then_parse_preserves_value(double seconds)
    {
        var original = Duration.FromSeconds(seconds);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Time, parsed.Unit.Dimension);
        Assert.Equal(original.Seconds, parsed.ToBase(), precision: 4);
    }

    // ===== Temperature =====

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(100)]
    [InlineData(-40)]
    public void Temperature_humanize_then_parse_preserves_value(double celsius)
    {
        var original = Temperature.FromCelsius(celsius);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Temperature, parsed.Unit.Dimension);
        Assert.Equal(original.Kelvin, parsed.ToBase(), precision: 2);
    }

    // ===== DataSize =====

    [Theory]
    [InlineData(1024L)]
    [InlineData(1536L)]
    [InlineData(1048576L)]
    [InlineData(1073741824L)]
    public void DataSize_humanize_then_parse_preserves_value(long bytes)
    {
        var original = DataSize.FromBytes(bytes);
        var text = original.Humanize();
        var parsed = H.Parse(text);
        Assert.Equal(Dimension.Data, parsed.Unit.Dimension);
        Assert.Equal(original.Bytes, (long)parsed.ToBase());
    }

    [Theory]
    [InlineData(1024L)]
    [InlineData(1073741824L)]
    public void DataSize_iec_round_trip_preserves_value(long bytes)
    {
        var original = DataSize.FromBytes(bytes);
        var opts = new HumanizeOptions { UseIecSymbols = true };
        var text = original.Humanize(opts);
        var parsed = H.Parse(text);
        Assert.Equal(original.Bytes, (long)parsed.ToBase());
    }

    // ===== Convert round-trip =====

    [Fact]
    public void Convert_there_and_back_is_identity()
    {
        var original = 1500.0;
        var kg = H.Convert(original, Units.Mass.Gram, Units.Mass.Kilogram);
        var back = H.Convert(kg, Units.Mass.Kilogram, Units.Mass.Gram);
        Assert.Equal(original, back, precision: 6);
    }

    [Fact]
    public void Convert_temperature_round_trip()
    {
        var celsius = 25.0;
        var fahrenheit = H.Convert(celsius, Units.Temperature.Celsius, Units.Temperature.Fahrenheit);
        var back = H.Convert(fahrenheit, Units.Temperature.Fahrenheit, Units.Temperature.Celsius);
        Assert.Equal(celsius, back, precision: 6);
    }

    // ===== Typed TryParse round-trip =====

    [Fact]
    public void TryParseDataSize_round_trip()
    {
        var original = DataSize.FromMegabytes(512);
        Assert.True(H.TryParseDataSize(original.Humanize(), out var restored));
        Assert.Equal(original.Bytes, restored.Bytes);
    }

    [Fact]
    public void TryParseMass_round_trip()
    {
        var original = Mass.FromKilograms(2);
        Assert.True(H.TryParseMass(original.Humanize(), out var restored));
        Assert.Equal(original.Grams, restored.Grams, precision: 4);
    }

    [Fact]
    public void TryParseTemperature_round_trip()
    {
        var original = Temperature.FromCelsius(100);
        Assert.True(H.TryParseTemperature(original.Humanize(), out var restored));
        Assert.Equal(original.Kelvin, restored.Kelvin, precision: 2);
    }
}

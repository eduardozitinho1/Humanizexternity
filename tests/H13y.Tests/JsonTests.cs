using System.Text.Json;
using H13y.Json;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class JsonTests
{
    // === Measure ===

    [Fact]
    public void Measure_serializes_to_object()
    {
        var m = H.Parse("1.5 kg");
        var json = JsonSerializer.Serialize(m, H13yJson.Options);
        Assert.Equal("{\"value\":1.5,\"unit\":\"kg\"}", json);
    }

    [Fact]
    public void Measure_round_trips()
    {
        var original = H.Parse("1.5 kg");
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Measure>(json, H13yJson.Options);
        Assert.Equal(original.Value, restored.Value);
        Assert.Equal(original.Unit.Symbol, restored.Unit.Symbol);
    }

    [Fact]
    public void Measure_deserialize_throws_on_missing_unit()
    {
        var json = "{\"value\":1.5}";
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<Measure>(json, H13yJson.Options));
    }

    // === DataSize ===

    [Fact]
    public void DataSize_serializes_as_number()
    {
        var json = JsonSerializer.Serialize(DataSize.FromMegabytes(1), H13yJson.Options);
        Assert.Equal("1048576", json);
    }

    [Fact]
    public void DataSize_round_trips()
    {
        var original = DataSize.FromGigabytes(2);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<DataSize>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }

    // === Mass ===

    [Fact]
    public void Mass_serializes_as_grams()
    {
        var json = JsonSerializer.Serialize(Mass.FromKilograms(1.5), H13yJson.Options);
        Assert.Equal("1500", json);
    }

    [Fact]
    public void Mass_round_trips()
    {
        var original = Mass.FromKilograms(1.5);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Mass>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }

    // === Length ===

    [Fact]
    public void Length_round_trips()
    {
        var original = Length.FromKilometers(1.5);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Length>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }

    // === Duration ===

    [Fact]
    public void Duration_round_trips()
    {
        var original = Duration.FromHours(1.5);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Duration>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }

    // === Volume ===

    [Fact]
    public void Volume_round_trips()
    {
        var original = Volume.FromLiters(1.5);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Volume>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }

    // === Area ===

    [Fact]
    public void Area_round_trips()
    {
        var original = Area.FromHectares(1);
        var json = JsonSerializer.Serialize(original, H13yJson.Options);
        var restored = JsonSerializer.Deserialize<Area>(json, H13yJson.Options);
        Assert.Equal(original, restored);
    }
}

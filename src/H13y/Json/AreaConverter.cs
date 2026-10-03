using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Area"/>. Serializes as a single JSON number
/// representing the area in square meters.
/// </summary>
public sealed class AreaConverter : JsonConverter<Area>
{
    public override Area Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetDouble());

    public override void Write(Utf8JsonWriter writer, Area value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.SquareMeters);
}

using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Length"/>. Serializes as a single JSON number
/// representing the length in meters.
/// </summary>
public sealed class LengthConverter : JsonConverter<Length>
{
    public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetDouble());

    public override void Write(Utf8JsonWriter writer, Length value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.Meters);
}

using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Volume"/>. Serializes as a single JSON number
/// representing the volume in liters.
/// </summary>
public sealed class VolumeConverter : JsonConverter<Volume>
{
    public override Volume Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetDouble());

    public override void Write(
        Utf8JsonWriter writer,
        Volume value,
        JsonSerializerOptions options
    ) => writer.WriteNumberValue(value.Liters);
}

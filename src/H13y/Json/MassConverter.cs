using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Mass"/>. Serializes as a single JSON number
/// representing the mass in grams.
/// </summary>
public sealed class MassConverter : JsonConverter<Mass>
{
    public override Mass Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetDouble());

    public override void Write(Utf8JsonWriter writer, Mass value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.Grams);
}

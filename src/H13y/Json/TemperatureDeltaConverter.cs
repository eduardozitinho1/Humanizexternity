using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

public sealed class TemperatureDeltaConverter : JsonConverter<TemperatureDelta>
{
    public override TemperatureDelta Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetDouble());

    public override void Write(
        Utf8JsonWriter writer,
        TemperatureDelta value,
        JsonSerializerOptions options
    ) => writer.WriteNumberValue(value.KelvinDelta);
}

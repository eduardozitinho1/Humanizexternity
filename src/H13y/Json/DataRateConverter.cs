using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="DataRate"/>. Serializes as a single JSON number
/// representing the rate in bits per second.
/// </summary>
public sealed class DataRateConverter : JsonConverter<DataRate>
{
    public override DataRate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetDouble());

    public override void Write(
        Utf8JsonWriter writer,
        DataRate value,
        JsonSerializerOptions options
    ) => writer.WriteNumberValue(value.BitsPerSecond);
}

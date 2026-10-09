using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Duration"/>. Serializes as a single JSON number
/// representing the duration in seconds.
/// </summary>
public sealed class DurationConverter : JsonConverter<Duration>
{
    public override Duration Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetDouble());

    public override void Write(
        Utf8JsonWriter writer,
        Duration value,
        JsonSerializerOptions options
    ) => writer.WriteNumberValue(value.Seconds);
}

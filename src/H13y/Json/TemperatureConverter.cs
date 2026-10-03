using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Temperature"/>. Serializes as a single JSON number
/// representing the temperature in kelvin (the base unit).
/// </summary>
public sealed class TemperatureConverter : JsonConverter<Temperature>
{
    public override Temperature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetDouble());

    public override void Write(Utf8JsonWriter writer, Temperature value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.Kelvin);
}

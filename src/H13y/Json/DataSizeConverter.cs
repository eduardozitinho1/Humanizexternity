using System.Text.Json;
using System.Text.Json.Serialization;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="DataSize"/>. Serializes as a single JSON number
/// representing the size in bytes, for example <c>1073741824</c>.
/// </summary>
public sealed class DataSizeConverter : JsonConverter<DataSize>
{
    public override DataSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DataSize value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.Bytes);
}

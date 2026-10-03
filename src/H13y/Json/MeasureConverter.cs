using System.Text.Json;
using System.Text.Json.Serialization;

namespace H13y.Json;

/// <summary>
/// JSON converter for <see cref="Measure"/>. Serializes to
/// <c>{"value": 1.5, "unit": "kg"}</c> and reads the same shape back.
/// </summary>
/// <remarks>
/// This converter is intentionally verbose so the payload is human-readable and
/// round-trips through other languages without losing the unit information.
/// For a more compact wire format, serialize typed measures such as
/// <see cref="Measures.Mass"/> directly — those store the base-unit value only
/// and rely on the type to imply the unit.
/// </remarks>
public sealed class MeasureConverter : JsonConverter<Measure>
{
    public override Measure Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected a JSON object for Measure.");

        double value = 0;
        string? unitSymbol = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            var property = reader.GetString();
            reader.Read();

            switch (property)
            {
                case "value":
                    value = reader.GetDouble();
                    break;
                case "unit":
                    unitSymbol = reader.GetString();
                    break;
            }
        }

        if (unitSymbol is null)
            throw new JsonException("Measure JSON is missing the 'unit' field.");

        var parsed = UnitParser.Parse($"{value} {unitSymbol}");
        return parsed;
    }

    public override void Write(Utf8JsonWriter writer, Measure value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("value", value.Value);
        writer.WriteString("unit", value.Unit.Symbol);
        writer.WriteEndObject();
    }
}

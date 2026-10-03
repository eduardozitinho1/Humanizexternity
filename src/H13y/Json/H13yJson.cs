using System.Text.Json;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// Provides ready-to-use <see cref="JsonSerializerOptions"/> with every H13y converter registered.
/// </summary>
/// <remarks>
/// The options instance is cached and thread-safe.
///
/// <code>
/// var json = JsonSerializer.Serialize(Temperature.FromCelsius(25), H13yJson.Options);
/// // "298.15"
///
/// var temp = JsonSerializer.Deserialize&lt;Temperature&gt;("298.15", H13yJson.Options);
/// </code>
/// </remarks>
public static class H13yJson
{
    /// <summary>Shared <see cref="JsonSerializerOptions"/> with all H13y converters registered.</summary>
    public static JsonSerializerOptions Options { get; } = Create();

    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new MeasureConverter());
        options.Converters.Add(new DataSizeConverter());
        options.Converters.Add(new MassConverter());
        options.Converters.Add(new LengthConverter());
        options.Converters.Add(new DurationConverter());
        options.Converters.Add(new VolumeConverter());
        options.Converters.Add(new AreaConverter());
        options.Converters.Add(new TemperatureConverter());
        return options;
    }
}

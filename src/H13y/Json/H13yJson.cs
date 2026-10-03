using System.Text.Json;
using H13y.Measures;

namespace H13y.Json;

/// <summary>
/// Provides ready-to-use <see cref="JsonSerializerOptions"/> with every H13y converter registered.
/// </summary>
/// <remarks>
/// Use this when you want to serialize or deserialize measures without registering each
/// converter by hand. The options instance is cached and thread-safe.
///
/// <code>
/// var json = JsonSerializer.Serialize(Mass.FromKilograms(1.5), H13yJson.Options);
/// // "1500"
///
/// var mass = JsonSerializer.Deserialize&lt;Mass&gt;("1500", H13yJson.Options);
/// </code>
/// </remarks>
public static class H13yJson
{
    /// <summary>
    /// Shared <see cref="JsonSerializerOptions"/> with all H13y converters registered.
    /// </summary>
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
        return options;
    }
}

namespace H13y;

/// <summary>
/// The scale used when humanizing a <see cref="Measures.Temperature"/> value.
/// </summary>
/// <remarks>
/// Temperature does not have a "best unit" the way mass or length do, because all three
/// scales are equally valid and used in different contexts. Humanization therefore requires
/// an explicit scale, defaulting to <see cref="Celsius"/> for everyday output.
/// </remarks>
public enum TemperatureScale
{
    /// <summary>Celsius (°C). Water freezes at 0 and boils at 100.</summary>
    Celsius,

    /// <summary>Fahrenheit (°F). Water freezes at 32 and boils at 212.</summary>
    Fahrenheit,

    /// <summary>Kelvin (K). SI base unit. Absolute zero is 0 K.</summary>
    Kelvin,
}

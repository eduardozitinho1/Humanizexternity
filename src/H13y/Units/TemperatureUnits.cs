namespace H13y;

public static partial class Units
{
    /// <summary>
    /// Temperature units. Unlike other dimensions, temperature is affine, not linear.
    /// </summary>
    /// <remarks>
    /// The base unit is the kelvin. Each unit carries a <see cref="Unit.Factor"/> and a
    /// <see cref="Unit.Offset"/> such that <c>kelvin = value * factor + offset</c>:
    ///
    /// <list type="bullet">
    ///   <item><description>Celsius: <c>K = C * 1 + 273.15</c>.</description></item>
    ///   <item><description>Fahrenheit: <c>K = F * 5/9 + (273.15 - 32 * 5/9)</c>.</description></item>
    ///   <item><description>Kelvin: identity.</description></item>
    /// </list>
    ///
    /// Because of the offset, the "best unit" concept does not apply here. The library
    /// rejects temperature in <see cref="BestUnitSelector"/> and requires an explicit scale
    /// when humanizing.
    /// </remarks>
    public static class Temperature
    {
        /// <summary>Celsius — 0 °C equals 273.15 K, water freezes at this point.</summary>
        public static readonly Unit Celsius = new("°C", 1, Dimension.Temperature, 273.15);

        /// <summary>Fahrenheit — water freezes at 32 °F and boils at 212 °F.</summary>
        public static readonly Unit Fahrenheit = new(
            "°F",
            5.0 / 9.0,
            Dimension.Temperature,
            273.15 - 32 * 5.0 / 9.0);

        /// <summary>Kelvin — the SI base unit of temperature. Absolute zero is 0 K.</summary>
        public static readonly Unit Kelvin = new("K", 1, Dimension.Temperature, 0);

        internal static readonly Unit[] All =
        [
            Celsius, Fahrenheit, Kelvin,
        ];
    }
}

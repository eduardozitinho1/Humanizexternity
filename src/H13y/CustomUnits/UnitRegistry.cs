namespace H13y;

/// <summary>
/// A mutable registry of user-defined units, consulted by the parser as a fallback
/// after the built-in catalog. Enables domain-specific units without forking the
/// library.
/// </summary>
/// <remarks>
/// Register units during application start-up, before any parsing runs in parallel.
/// The registry is not thread-safe for concurrent writes; reads are safe after
/// registration is complete.
///
/// Custom units are available to <see cref="H.Parse(string, System.Globalization.CultureInfo)"/>
/// and <see cref="H.Convert(string, Unit, System.Globalization.CultureInfo)"/>, but are
/// never auto-selected by <see cref="H.Best(double, Dimension, HumanizeOptions?)"/>
/// because they are not part of <see cref="Units.All"/>.
/// </remarks>
public sealed class UnitRegistry
{
    private readonly Dictionary<string, Unit> _units = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers a custom unit and returns the registry for chaining.
    /// </summary>
    /// <param name="symbol">Display symbol, e.g. "widget".</param>
    /// <param name="factor">Multiplier to convert this unit to the base unit of <paramref name="dimension"/>.</param>
    /// <param name="dimension">The dimension this unit belongs to.</param>
    /// <param name="iecSymbol">Optional alternative symbol for IEC-style display.</param>
    /// <param name="aliases">Additional aliases recognized by the parser.</param>
    public UnitRegistry Register(
        string symbol,
        double factor,
        Dimension dimension,
        string? iecSymbol = null,
        params string[] aliases
    )
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol must not be empty.", nameof(symbol));
        if (factor <= 0)
            throw new ArgumentOutOfRangeException(nameof(factor), factor, "Factor must be positive.");

        var unit = new Unit(symbol, factor, dimension, 0, iecSymbol);
        _units[symbol] = unit;

        if (iecSymbol is not null)
            _units[iecSymbol] = unit;

        foreach (var alias in aliases)
            if (!string.IsNullOrWhiteSpace(alias))
                _units[alias] = unit;

        return this;
    }

    /// <summary>
    /// Attempts to resolve a symbol or alias to a registered custom unit.
    /// </summary>
    public Unit? Resolve(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return null;
        return _units.TryGetValue(symbol.Trim(), out var unit) ? unit : null;
    }

    /// <summary>
    /// Removes a custom unit. Returns true when something was removed.
    /// </summary>
    public bool Unregister(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return false;
        return _units.Remove(symbol.Trim());
    }

    /// <summary>Removes every custom unit.</summary>
    public void Clear() => _units.Clear();

    /// <summary>Lists every registered custom unit. Useful for diagnostics.</summary>
    public IReadOnlyCollection<Unit> All =>
        _units.Values.Distinct().ToArray();
}

public static partial class H
{
    /// <summary>
    /// Global registry consulted by the parser as a fallback after the built-in
    /// unit catalog. Register custom units here to make them available to every
    /// <see cref="H.Parse(string, System.Globalization.CultureInfo)"/> call.
    /// </summary>
    public static UnitRegistry GlobalUnits { get; } = new();
}

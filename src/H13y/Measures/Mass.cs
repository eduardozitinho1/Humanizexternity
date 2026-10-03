namespace H13y.Measures;

/// <summary>
/// A quantity of mass, stored internally as a number of grams.
/// </summary>
/// <remarks>
/// <see cref="Mass"/> represents weights and amounts of matter. The base unit is the gram
/// because it gives a good balance between everyday values (a few hundred grams) and
/// scientific ones (milligrams), while still supporting very large values through tonnes.
///
/// The stored value is a <see cref="double"/>, so fractions are preserved exactly as
/// provided. For applications that require exact decimal arithmetic (financial or
/// laboratory scenarios), consider rounding at the boundary or using a dedicated
/// decimal type on top.
/// </remarks>
/// <param name="Grams">The mass in grams.</param>
public readonly record struct Mass(double Grams)
{
    /// <summary>Creates a <see cref="Mass"/> from a number of milligrams.</summary>
    public static Mass FromMilligrams(double mg) => new(mg / 1000);

    /// <summary>Creates a <see cref="Mass"/> from a number of grams.</summary>
    public static Mass FromGrams(double g) => new(g);

    /// <summary>Creates a <see cref="Mass"/> from a number of kilograms.</summary>
    public static Mass FromKilograms(double kg) => new(kg * 1000);

    /// <summary>Creates a <see cref="Mass"/> from a number of metric tonnes.</summary>
    public static Mass FromTonnes(double t) => new(t * 1_000_000);

    /// <summary>Returns the mass expressed in milligrams.</summary>
    public double ToMilligrams() => Grams * 1000;

    /// <summary>Returns the mass expressed in kilograms.</summary>
    public double ToKilograms() => Grams / 1000;

    /// <summary>Returns the mass expressed in metric tonnes.</summary>
    public double ToTonnes() => Grams / 1_000_000;

    /// <summary>
    /// Renders the mass as a human-readable string such as "1.5 kg" or "500 g".
    /// </summary>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Grams, Dimension.Mass, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

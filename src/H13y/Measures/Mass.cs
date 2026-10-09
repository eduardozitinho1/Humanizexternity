namespace H13y.Measures;

/// <summary>
/// A quantity of mass, stored internally as a number of grams.
/// </summary>
/// <param name="Grams">The mass in grams.</param>
public readonly record struct Mass(double Grams) : IComparable<Mass>, IFormattable
{
    public static Mass FromMilligrams(double mg) => new(mg / 1000);

    public static Mass FromGrams(double g) => new(g);

    public static Mass FromKilograms(double kg) => new(kg * 1000);

    public static Mass FromTonnes(double t) => new(t * 1_000_000);

    public double ToMilligrams() => Grams * 1000;

    public double ToKilograms() => Grams / 1000;

    public double ToTonnes() => Grams / 1_000_000;

    public static Mass operator +(Mass left, Mass right) => new(left.Grams + right.Grams);

    public static Mass operator -(Mass left, Mass right) => new(left.Grams - right.Grams);

    public static bool operator <(Mass left, Mass right) => left.Grams < right.Grams;

    public static bool operator <=(Mass left, Mass right) => left.Grams <= right.Grams;

    public static bool operator >(Mass left, Mass right) => left.Grams > right.Grams;

    public static bool operator >=(Mass left, Mass right) => left.Grams >= right.Grams;

    public int CompareTo(Mass other) => Grams.CompareTo(other.Grams);

    public string Humanize(HumanizeOptions? options = null) =>
        HumanizeFormatter.Format(Grams, Dimension.Mass, options);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        FormattableHelpers.FormatScalar(Grams, Dimension.Mass, format, formatProvider);

    public override string ToString() => Humanize();
}

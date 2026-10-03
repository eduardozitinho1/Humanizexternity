namespace H13y.Measures;

/// <summary>Mass, stored as grams.</summary>
public readonly record struct Mass(double Grams)
{
    public static Mass FromMilligrams(double mg) => new(mg / 1000);
    public static Mass FromGrams(double g) => new(g);
    public static Mass FromKilograms(double kg) => new(kg * 1000);
    public static Mass FromTonnes(double t) => new(t * 1_000_000);

    public double ToMilligrams() => Grams * 1000;
    public double ToKilograms() => Grams / 1000;
    public double ToTonnes() => Grams / 1_000_000;

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Grams, Dimension.Mass, options);

    public override string ToString() => Humanize();
}

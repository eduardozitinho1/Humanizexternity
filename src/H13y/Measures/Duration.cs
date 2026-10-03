namespace H13y.Measures;

/// <summary>Duration of time, stored as seconds.</summary>
public readonly record struct Duration(double Seconds)
{
    public static Duration FromMilliseconds(double ms) => new(ms / 1000);
    public static Duration FromSeconds(double s) => new(s);
    public static Duration FromMinutes(double m) => new(m * 60);
    public static Duration FromHours(double h) => new(h * 3600);
    public static Duration FromDays(double d) => new(d * 86400);
    public static Duration FromWeeks(double w) => new(w * 604800);

    public double ToMilliseconds() => Seconds * 1000;
    public double ToMinutes() => Seconds / 60;
    public double ToHours() => Seconds / 3600;
    public double ToDays() => Seconds / 86400;
    public double ToWeeks() => Seconds / 604800;

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.FormatDuration(Seconds, options);

    public override string ToString() => Humanize();
}

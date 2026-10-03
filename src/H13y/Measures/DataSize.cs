namespace H13y.Measures;

/// <summary>Digital information size, stored as bytes.</summary>
public readonly record struct DataSize(long Bytes)
{
    public static DataSize FromBytes(long bytes) => new(bytes);
    public static DataSize FromKilobytes(double kb) => new((long)(kb * 1024));
    public static DataSize FromMegabytes(double mb) => new((long)(mb * 1024 * 1024));
    public static DataSize FromGigabytes(double gb) => new((long)(gb * 1024 * 1024 * 1024));
    public static DataSize FromTerabytes(double tb) => new((long)(tb * 1024 * 1024 * 1024 * 1024));

    public double ToKilobytes() => Bytes / 1024.0;
    public double ToMegabytes() => Bytes / (1024.0 * 1024);
    public double ToGigabytes() => Bytes / (1024.0 * 1024 * 1024);
    public double ToTerabytes() => Bytes / (1024.0 * 1024 * 1024 * 1024);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Bytes, Dimension.Data, options);

    public override string ToString() => Humanize();
}

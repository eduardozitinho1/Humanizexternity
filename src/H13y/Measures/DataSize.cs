namespace H13y.Measures;

/// <summary>
/// A quantity of digital information, stored internally as a number of bytes.
/// </summary>
/// <remarks>
/// <see cref="DataSize"/> is the strongly-typed representation for anything measured in
/// bytes: file sizes, memory usage, network traffic, database volumes. Internally it
/// always stores the value as a <see cref="long"/> count of bytes, so arithmetic is exact
/// and there is no risk of losing precision.
/// </remarks>
/// <param name="Bytes">The size in bytes.</param>
public readonly record struct DataSize(long Bytes) : IComparable<DataSize>
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

    public static DataSize operator +(DataSize left, DataSize right) => new(left.Bytes + right.Bytes);
    public static DataSize operator -(DataSize left, DataSize right) => new(left.Bytes - right.Bytes);
    public static bool operator <(DataSize left, DataSize right) => left.Bytes < right.Bytes;
    public static bool operator <=(DataSize left, DataSize right) => left.Bytes <= right.Bytes;
    public static bool operator >(DataSize left, DataSize right) => left.Bytes > right.Bytes;
    public static bool operator >=(DataSize left, DataSize right) => left.Bytes >= right.Bytes;

    public int CompareTo(DataSize other) => Bytes.CompareTo(other.Bytes);

    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Bytes, Dimension.Data, options);

    public override string ToString() => Humanize();
}

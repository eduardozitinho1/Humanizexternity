namespace H13y.Measures;

/// <summary>
/// A quantity of digital information, stored internally as a number of bytes.
/// </summary>
/// <remarks>
/// <see cref="DataSize"/> is the strongly-typed representation for anything measured in
/// bytes: file sizes, memory usage, network traffic, database volumes. Internally it
/// always stores the value as a <see cref="long"/> count of bytes, so arithmetic is exact
/// and there is no risk of losing precision through floating-point conversions.
///
/// Create instances using the <c>From*</c> factory methods and inspect them using the
/// <c>To*</c> methods. To render a value for display, call <see cref="Humanize"/>, which
/// picks the best unit automatically (for example, <c>1073741824</c> bytes becomes "1 GB").
///
/// This type is a <c>readonly record struct</c>, so it is immutable and cheap to copy.
/// </remarks>
/// <param name="Bytes">The size in bytes.</param>
public readonly record struct DataSize(long Bytes)
{
    /// <summary>Creates a <see cref="DataSize"/> from a raw byte count.</summary>
    /// <param name="bytes">The number of bytes.</param>
    public static DataSize FromBytes(long bytes) => new(bytes);

    /// <summary>Creates a <see cref="DataSize"/> from a number of kilobytes (1024 bytes each).</summary>
    /// <param name="kb">The number of kilobytes.</param>
    public static DataSize FromKilobytes(double kb) => new((long)(kb * 1024));

    /// <summary>Creates a <see cref="DataSize"/> from a number of megabytes (1024² bytes each).</summary>
    /// <param name="mb">The number of megabytes.</param>
    public static DataSize FromMegabytes(double mb) => new((long)(mb * 1024 * 1024));

    /// <summary>Creates a <see cref="DataSize"/> from a number of gigabytes (1024³ bytes each).</summary>
    /// <param name="gb">The number of gigabytes.</param>
    public static DataSize FromGigabytes(double gb) => new((long)(gb * 1024 * 1024 * 1024));

    /// <summary>Creates a <see cref="DataSize"/> from a number of terabytes (1024⁴ bytes each).</summary>
    /// <param name="tb">The number of terabytes.</param>
    public static DataSize FromTerabytes(double tb) => new((long)(tb * 1024 * 1024 * 1024 * 1024));

    /// <summary>Returns the size expressed in kilobytes.</summary>
    public double ToKilobytes() => Bytes / 1024.0;

    /// <summary>Returns the size expressed in megabytes.</summary>
    public double ToMegabytes() => Bytes / (1024.0 * 1024);

    /// <summary>Returns the size expressed in gigabytes.</summary>
    public double ToGigabytes() => Bytes / (1024.0 * 1024 * 1024);

    /// <summary>Returns the size expressed in terabytes.</summary>
    public double ToTerabytes() => Bytes / (1024.0 * 1024 * 1024 * 1024);

    /// <summary>
    /// Renders the size as a human-readable string such as "1 GB" or "1.5 KB".
    /// </summary>
    /// <param name="options">Optional formatting options. When omitted, <see cref="HumanizeOptions.Default"/> is used.</param>
    /// <returns>A compact, user-friendly representation of the size.</returns>
    public string Humanize(HumanizeOptions? options = null)
        => HumanizeFormatter.Format(Bytes, Dimension.Data, options);

    /// <summary>Equivalent to <see cref="Humanize"/> with default options.</summary>
    public override string ToString() => Humanize();
}

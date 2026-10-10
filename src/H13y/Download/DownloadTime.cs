using H13y.Measures;

namespace H13y;

public static partial class H
{
    /// <summary>
    /// Computes how long a download will take given the size in bytes and the
    /// transfer rate in bits per second. Returns a <see cref="Duration"/> so the
    /// caller can humanize it however they like.
    /// </summary>
    public static Duration DownloadDuration(long bytes, double bitsPerSecond)
    {
        if (bytes < 0)
            throw new ArgumentOutOfRangeException(
                nameof(bytes),
                bytes,
                "Bytes must not be negative."
            );
        if (bitsPerSecond <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(bitsPerSecond),
                bitsPerSecond,
                "Bits per second must be positive."
            );

        var seconds = bytes * 8.0 / bitsPerSecond;
        return Duration.FromSeconds(seconds);
    }

    /// <summary>
    /// Computes how long a download will take given the size and transfer rate.
    /// </summary>
    public static Duration DownloadDuration(DataSize size, Bits rate) =>
        DownloadDuration(size.Bytes, rate.Value);

    /// <summary>
    /// Computes and humanizes how long a download will take: "3.6 s".
    /// </summary>
    public static string DownloadTime(
        long bytes,
        double bitsPerSecond,
        HumanizeOptions? options = null
    ) => DownloadDuration(bytes, bitsPerSecond).Humanize(options);

    /// <summary>
    /// Computes and humanizes how long a download will take.
    /// </summary>
    public static string DownloadTime(DataSize size, Bits rate, HumanizeOptions? options = null) =>
        DownloadDuration(size, rate).Humanize(options);
}

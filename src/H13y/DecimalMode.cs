namespace H13y;

/// <summary>
/// Determines the scaling factor used when converting between data units.
/// </summary>
/// <remarks>
/// There are two conventions in the industry:
///
/// <list type="bullet">
///   <item><description><b>Binary</b> — 1 KB = 1024 bytes. Used by operating systems, RAM, and most developer tools.</description></item>
///   <item><description><b>Metric</b> — 1 kB = 1000 bytes. The SI standard, used by storage manufacturers.</description></item>
/// </list>
///
/// Historically, "kilobyte" meant 1024 bytes in computing, but the IEC introduced
/// KiB/MiB/GiB to disambiguate. This library defaults to <see cref="Binary"/> because
/// that is what developers usually expect when humanizing memory and file sizes.
/// Use <see cref="Metric"/> when producing user-facing output for storage vendors
/// or any context that follows the SI standard.
/// </remarks>
public enum DecimalMode
{
    /// <summary>
    /// 1024-based scaling (1 KB = 1024 B, 1 MB = 1024 KB). Default for data.
    /// </summary>
    Binary,

    /// <summary>
    /// 1000-based scaling (1 kB = 1000 B, 1 MB = 1000 kB). SI standard.
    /// </summary>
    Metric,
}

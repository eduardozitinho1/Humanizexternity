namespace H13y;

/// <summary>
/// Centralizes argument validation so throwing logic is consistent across the library.
/// </summary>
/// <remarks>
/// Keeping throws in one place makes the calling code cleaner and avoids duplicated
/// conditional logic. It also gives the library a single spot to evolve toward
/// .NET's newer guard APIs (<c>ArgumentOutOfRangeException.ThrowIfNegative</c>) without
/// touching every call site.
/// </remarks>
internal static class ThrowHelper
{
    /// <summary>
    /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/> is negative.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="paramName">The parameter name to include in the exception.</param>
    public static void ThrowIfNegative(double value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, "Value must not be negative.");
    }
}

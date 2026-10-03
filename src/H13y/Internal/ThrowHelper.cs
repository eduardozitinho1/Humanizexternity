namespace H13y;

internal static class ThrowHelper
{
    public static void ThrowIfNegative(double value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, "Value must not be negative.");
    }
}

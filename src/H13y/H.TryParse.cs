using System.Globalization;
using H13y.Measures;

namespace H13y;

public static partial class H
{
    public static bool TryParseSpeed(string text, CultureInfo culture, out Speed speed)
    {
        if (TryParseDimension(text, culture, Dimension.Speed, out var value))
        {
            speed = Speed.FromMetersPerSecond(value);
            return true;
        }
        speed = default;
        return false;
    }

    public static bool TryParseEnergy(string text, CultureInfo culture, out Energy energy)
    {
        if (TryParseDimension(text, culture, Dimension.Energy, out var value))
        {
            energy = Energy.FromJoules(value);
            return true;
        }
        energy = default;
        return false;
    }

    public static bool TryParsePower(string text, CultureInfo culture, out Power power)
    {
        if (TryParseDimension(text, culture, Dimension.Power, out var value))
        {
            power = Power.FromWatts(value);
            return true;
        }
        power = default;
        return false;
    }

    public static bool TryParsePressure(string text, CultureInfo culture, out Pressure pressure)
    {
        if (TryParseDimension(text, culture, Dimension.Pressure, out var value))
        {
            pressure = Pressure.FromPascals(value);
            return true;
        }
        pressure = default;
        return false;
    }

    public static bool TryParseFrequency(string text, CultureInfo culture, out Frequency frequency)
    {
        if (TryParseDimension(text, culture, Dimension.Frequency, out var value))
        {
            frequency = Frequency.FromHertz(value);
            return true;
        }
        frequency = default;
        return false;
    }

    public static bool TryParseAngle(string text, CultureInfo culture, out Angle angle)
    {
        if (TryParseDimension(text, culture, Dimension.Angle, out var value))
        {
            angle = Angle.FromRadians(value);
            return true;
        }
        angle = default;
        return false;
    }

    public static bool TryParseBits(string text, CultureInfo culture, out Measures.Bits bits)
    {
        if (TryParseDimension(text, culture, Dimension.Bits, out var value))
        {
            bits = Measures.Bits.FromBits(value);
            return true;
        }
        bits = default;
        return false;
    }

    public static bool TryParseDataRate(string text, CultureInfo culture, out DataRate rate)
    {
        if (TryParseDimension(text, culture, Dimension.DataRate, out var value))
        {
            rate = DataRate.FromBitsPerSecond(value);
            return true;
        }
        rate = default;
        return false;
    }
}

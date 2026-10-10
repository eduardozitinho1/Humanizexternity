using System.Globalization;
using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class IecBitsTests
{
    [Fact]
    public void Kibibit_parses_to_1024_bits()
    {
        var m = H.Parse("1 Kib", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Bits, m.Unit.Dimension);
        Assert.Equal(1_024, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Mebibit_parses_to_1048576_bits()
    {
        var m = H.Parse("1 Mib", CultureInfo.InvariantCulture);
        Assert.Equal(1_048_576, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Kibibit_via_full_name()
    {
        var m = H.Parse("1 kibibit", CultureInfo.InvariantCulture);
        Assert.Equal(1_024, m.ToBase(), precision: 6);
    }

    [Fact]
    public void Kib_and_KiB_resolve_to_different_dimensions()
    {
        var kib = H.Parse("1 Kib", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Bits, kib.Unit.Dimension);

        var KiB = H.Parse("1 KiB", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Data, KiB.Unit.Dimension);
    }

    [Fact]
    public void FromKibibits_helper()
    {
        var bits = Measures.Bits.FromBits(2_048);
        Assert.Equal(2, bits.ToKilobits() / 1_024 * 1_000, precision: 6);
    }

    [Fact]
    public void Convert_kibibit_to_kilobit() =>
        Assert.Equal(
            1.024,
            H.Convert(1, Units.Bits.Kibibit, Units.Bits.Kilobit),
            precision: 6
        );
}

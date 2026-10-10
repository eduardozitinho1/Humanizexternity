using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class ApproximateTests
{
    [Fact]
    public void Hundred_rounds_to_nearest_hundred() =>
        Assert.Equal("about 1200", H.Approximate(1234, ApproximationPrecision.Hundred));

    [Fact]
    public void Tenth_rounds_to_one_decimal() =>
        Assert.Equal("about 0.8", H.Approximate(0.784, ApproximationPrecision.Tenth));

    [Fact]
    public void Million_rounds_to_nearest_million() =>
        Assert.Equal(
            "about 2 million",
            H.Approximate(1_500_000, ApproximationPrecision.Million)
        );

    [Fact]
    public void TenThousand_uses_compact_suffix() =>
        Assert.Equal(
            "about 15 thousand",
            H.Approximate(15_000, ApproximationPrecision.Thousand)
        );

    [Fact]
    public void Negative_values_keep_sign() =>
        Assert.Equal(
            "about -1200",
            H.Approximate(-1234, ApproximationPrecision.Hundred)
        );

    [Fact]
    public void Zero_is_reported_as_zero() =>
        Assert.Equal("about 0", H.Approximate(0, ApproximationPrecision.One));

    [Fact]
    public void NaN_returns_about_NaN() =>
        Assert.Equal("about NaN", H.Approximate(double.NaN, ApproximationPrecision.One));

    [Fact]
    public void Positive_infinity_returns_about_infinity() =>
        Assert.Equal("about ∞", H.Approximate(double.PositiveInfinity, ApproximationPrecision.One));

    [Fact]
    public void Negative_infinity_returns_about_negative_infinity() =>
        Assert.Equal(
            "about -∞",
            H.Approximate(double.NegativeInfinity, ApproximationPrecision.One)
        );

    [Fact]
    public void Roughly_style_changes_prefix() =>
        Assert.Equal(
            "roughly 1200",
            H.Approximate(1234, ApproximationPrecision.Hundred, ApproximationStyle.Roughly)
        );

    [Fact]
    public void Approximately_style_changes_prefix() =>
        Assert.Equal(
            "approximately 1200",
            H.Approximate(
                1234,
                ApproximationPrecision.Hundred,
                ApproximationStyle.Approximately
            )
        );

    [Fact]
    public void Billion_uses_billion_suffix() =>
        Assert.Equal(
            "about 3 billion",
            H.Approximate(2_800_000_000, ApproximationPrecision.Billion)
        );

    [Fact]
    public void Culture_is_honored_in_plain_format()
    {
        var opts = new HumanizeOptions { Culture = new CultureInfo("pt-BR") };
        Assert.Equal(
            "about 1234,6",
            H.Approximate(1234.567, ApproximationPrecision.Tenth, opts)
        );
    }
}

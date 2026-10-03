using H13y.Measures;
using Xunit;

namespace H13y.Tests;

public class ArithmeticTests
{
    // === DataSize ===

    [Fact]
    public void DataSize_addition_sums_bytes()
    {
        var a = DataSize.FromMegabytes(500);
        var b = DataSize.FromMegabytes(700);
        Assert.Equal(1200 * 1024 * 1024, (a + b).Bytes);
    }

    [Fact]
    public void DataSize_subtraction_returns_difference()
    {
        var a = DataSize.FromGigabytes(2);
        var b = DataSize.FromMegabytes(500);
        Assert.Equal(1548 * 1024 * 1024, (a - b).Bytes);
    }

    [Fact]
    public void DataSize_comparison_orders_by_bytes()
    {
        var small = DataSize.FromKilobytes(1);
        var big = DataSize.FromMegabytes(1);
        Assert.True(big > small);
        Assert.True(small < big);
        Assert.False(small >= big);
    }

    // === Mass ===

    [Fact]
    public void Mass_addition_sums_grams()
    {
        var a = Mass.FromKilograms(1.5);
        var b = Mass.FromGrams(500);
        Assert.Equal(2000, (a + b).Grams);
    }

    [Fact]
    public void Mass_subtraction_returns_difference()
    {
        var a = Mass.FromKilograms(2);
        var b = Mass.FromGrams(750);
        Assert.Equal(1250, (a - b).Grams);
    }

    [Fact]
    public void Mass_comparison_orders_by_grams()
    {
        var small = Mass.FromGrams(500);
        var big = Mass.FromKilograms(1);
        Assert.True(big > small);
        Assert.True(small <= big);
    }

    // === Length ===

    [Fact]
    public void Length_addition_sums_meters()
    {
        var a = Length.FromKilometers(1);
        var b = Length.FromMeters(500);
        Assert.Equal(1500, (a + b).Meters);
    }

    [Fact]
    public void Length_comparison_orders_by_meters()
    {
        var small = Length.FromCentimeters(50);
        var big = Length.FromMeters(1);
        Assert.True(big > small);
    }

    // === Duration ===

    [Fact]
    public void Duration_addition_sums_seconds()
    {
        var a = Duration.FromMinutes(90);
        var b = Duration.FromMinutes(30);
        Assert.Equal(7200, (a + b).Seconds);
    }

    [Fact]
    public void Duration_comparison_orders_by_seconds()
    {
        var small = Duration.FromMinutes(30);
        var big = Duration.FromHours(1);
        Assert.True(big > small);
        Assert.True(small < big);
    }

    // === Volume ===

    [Fact]
    public void Volume_addition_sums_liters()
    {
        var a = Volume.FromLiters(1.5);
        var b = Volume.FromMilliliters(500);
        Assert.Equal(2.0, (a + b).Liters, precision: 6);
    }

    [Fact]
    public void Volume_comparison_orders_by_liters()
    {
        var small = Volume.FromMilliliters(500);
        var big = Volume.FromLiters(1);
        Assert.True(big > small);
    }

    // === Area ===

    [Fact]
    public void Area_addition_sums_square_meters()
    {
        var a = Area.FromHectares(1);
        var b = Area.FromSquareMeters(5000);
        Assert.Equal(15_000, (a + b).SquareMeters);
    }

    [Fact]
    public void Area_comparison_orders_by_square_meters()
    {
        var small = Area.FromSquareMeters(100);
        var big = Area.FromHectares(1);
        Assert.True(big > small);
    }

    // === Equality (já vem do record struct) ===

    [Fact]
    public void Mass_equality_works_across_units()
    {
        var a = Mass.FromKilograms(1);
        var b = Mass.FromGrams(1000);
        Assert.Equal(a, b);
    }

    [Fact]
    public void DataSize_equality_works_across_units()
    {
        var a = DataSize.FromKilobytes(1);
        var b = DataSize.FromBytes(1024);
        Assert.Equal(a, b);
    }
}

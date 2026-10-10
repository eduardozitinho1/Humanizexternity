using System.Globalization;
using Xunit;

namespace H13y.Tests;

public class UnitRegistryTests
{
    [Fact]
    public void Custom_unit_parses()
    {
        var registry = new UnitRegistry();
        registry.Register("widget", factor: 250, dimension: Dimension.Mass);
        registry.Register("gadget", factor: 125, dimension: Dimension.Mass, aliases: "gadgets");

        var unit = registry.Resolve("widget");
        Assert.NotNull(unit);
        Assert.Equal(250, unit!.Factor);
        Assert.Equal(Dimension.Mass, unit.Dimension);

        var alias = registry.Resolve("gadgets");
        Assert.NotNull(alias);
        Assert.Equal(125, alias!.Factor);
    }

    [Fact]
    public void Custom_unit_is_case_insensitive()
    {
        var registry = new UnitRegistry();
        registry.Register("widget", 250, Dimension.Mass);
        Assert.NotNull(registry.Resolve("WIDGET"));
        Assert.NotNull(registry.Resolve("Widget"));
    }

    [Fact]
    public void Global_units_are_visible_to_parser()
    {
        H.GlobalUnits.Register("gizmo", 1_000_000, Dimension.Mass, aliases: "gizmos");

        var m = H.Parse("2 gizmos", CultureInfo.InvariantCulture);
        Assert.Equal(Dimension.Mass, m.Unit.Dimension);
        Assert.Equal(2_000_000, m.ToBase(), precision: 6);

        H.GlobalUnits.Unregister("gizmo");
        H.GlobalUnits.Unregister("gizmos");
    }

    [Fact]
    public void Unregister_removes_the_unit()
    {
        var registry = new UnitRegistry();
        registry.Register("tempunit", 1, Dimension.Mass);
        Assert.True(registry.Unregister("tempunit"));
        Assert.Null(registry.Resolve("tempunit"));
        Assert.False(registry.Unregister("tempunit"));
    }

    [Fact]
    public void Clear_removes_everything()
    {
        var registry = new UnitRegistry();
        registry.Register("a", 1, Dimension.Mass);
        registry.Register("b", 2, Dimension.Mass);
        registry.Clear();
        Assert.Null(registry.Resolve("a"));
        Assert.Null(registry.Resolve("b"));
    }

    [Fact]
    public void Negative_factor_throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new UnitRegistry().Register("bad", -1, Dimension.Mass)
        );

    [Fact]
    public void Empty_symbol_throws() =>
        Assert.Throws<ArgumentException>(() =>
            new UnitRegistry().Register("", 1, Dimension.Mass)
        );
}

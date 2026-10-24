using Xunit;

namespace H13y.Tests;

public class InflectorTests
{
    [Theory]
    [InlineData("cat", "cats")]
    [InlineData("dog", "dogs")]
    [InlineData("box", "boxes")]
    [InlineData("church", "churches")]
    [InlineData("city", "cities")]
    [InlineData("baby", "babies")]
    [InlineData("leaf", "leaves")]
    [InlineData("wolf", "wolves")]
    [InlineData("analysis", "analyses")]
    [InlineData("quiz", "quizzes")]
    [InlineData("tomato", "tomatoes")]
    [InlineData("status", "statuses")]
    public void Pluralize_handles_regular_words(string singular, string plural) =>
        Assert.Equal(plural, H.Pluralize(singular));

    [Theory]
    [InlineData("person", "people")]
    [InlineData("man", "men")]
    [InlineData("woman", "women")]
    [InlineData("child", "children")]
    [InlineData("tooth", "teeth")]
    [InlineData("foot", "feet")]
    [InlineData("mouse", "mice")]
    [InlineData("criterion", "criteria")]
    [InlineData("cactus", "cacti")]
    public void Pluralize_handles_irregulars(string singular, string plural) =>
        Assert.Equal(plural, H.Pluralize(singular));

    [Theory]
    [InlineData("sheep")]
    [InlineData("fish")]
    [InlineData("deer")]
    [InlineData("series")]
    [InlineData("species")]
    public void Pluralize_leaves_uncountables_unchanged(string word) =>
        Assert.Equal(word, H.Pluralize(word));

    [Theory]
    [InlineData("cats", "cat")]
    [InlineData("boxes", "box")]
    [InlineData("cities", "city")]
    [InlineData("leaves", "leaf")]
    [InlineData("people", "person")]
    [InlineData("children", "child")]
    [InlineData("mice", "mouse")]
    public void Singularize_handles_plurals(string plural, string singular) =>
        Assert.Equal(singular, H.Singularize(plural));

    [Fact]
    public void Pluralize_preserves_capitalization() =>
        Assert.Equal("People", H.Pluralize("Person"));

    [Fact]
    public void Pluralize_uppercase_preserved() =>
        Assert.Equal("CHILDREN", H.Pluralize("CHILD"));

    [Fact]
    public void Pluralize_round_trips_through_singularize()
    {
        var words = new[] { "cat", "box", "city", "person", "analysis", "wolf" };
        foreach (var w in words)
            Assert.Equal(w, H.Singularize(H.Pluralize(w)));
    }

    [Fact]
    public void IsPlural_detects_plurals()
    {
        Assert.True(H.IsPlural("cats"));
        Assert.True(H.IsPlural("people"));
        Assert.False(H.IsPlural("cat"));
    }

    [Fact]
    public void IsSingular_detects_singulars()
    {
        Assert.True(H.IsSingular("cat"));
        Assert.True(H.IsSingular("person"));
        Assert.False(H.IsSingular("cats"));
    }

    [Fact]
    public void Pluralize_empty_returns_empty() =>
        Assert.Equal("", H.Pluralize(""));
}

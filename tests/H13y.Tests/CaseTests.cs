using Xunit;

namespace H13y.Tests;

public class CaseTests
{
    [Theory]
    [InlineData("fooBar", "foo_bar")]
    [InlineData("FooBar", "foo_bar")]
    [InlineData("foo bar", "foo_bar")]
    [InlineData("foo-bar", "foo_bar")]
    [InlineData("HTTPServer", "http_server")]
    [InlineData("already_snake", "already_snake")]
    public void SnakeCase(string input, string expected) =>
        Assert.Equal(expected, H.SnakeCase(input));

    [Theory]
    [InlineData("fooBar", "foo-bar")]
    [InlineData("FooBar", "foo-bar")]
    [InlineData("HTTPServer", "http-server")]
    public void KebabCase(string input, string expected) =>
        Assert.Equal(expected, H.KebabCase(input));

    [Theory]
    [InlineData("foo_bar", "fooBar")]
    [InlineData("FooBar", "fooBar")]
    [InlineData("foo-bar", "fooBar")]
    [InlineData("foo bar", "fooBar")]
    public void CamelCase(string input, string expected) =>
        Assert.Equal(expected, H.CamelCase(input));

    [Theory]
    [InlineData("foo_bar", "FooBar")]
    [InlineData("foo bar", "FooBar")]
    [InlineData("fooBar", "FooBar")]
    public void PascalCase(string input, string expected) =>
        Assert.Equal(expected, H.PascalCase(input));

    [Theory]
    [InlineData("foo_bar", "Foo Bar")]
    [InlineData("fooBar", "Foo Bar")]
    public void TitleCase(string input, string expected) =>
        Assert.Equal(expected, H.TitleCase(input));

    [Theory]
    [InlineData("foo_bar", "FOO_BAR")]
    [InlineData("FooBar", "FOO_BAR")]
    public void UpperSnakeCase(string input, string expected) =>
        Assert.Equal(expected, H.UpperSnakeCase(input));

    [Theory]
    [InlineData("foo_bar", "Foo bar")]
    [InlineData("FooBar", "Foo bar")]
    [InlineData("HTTPServer", "Http server")]
    public void Humanize(string input, string expected) =>
        Assert.Equal(expected, H.Humanize(input));

    [Fact]
    public void SnakeCase_empty() =>
        Assert.Equal(string.Empty, H.SnakeCase(""));

    [Fact]
    public void CamelCase_round_trip() =>
        Assert.Equal("fooBar", H.CamelCase(H.SnakeCase("fooBar")));
}

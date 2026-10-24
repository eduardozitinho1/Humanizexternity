using H13y.Localization;
using Xunit;

namespace H13y.Tests;

public class PortugueseLanguageTests
{
    [Theory]
    [InlineData(0, "zero")]
    [InlineData(1, "um")]
    [InlineData(2, "dois")]
    [InlineData(15, "quinze")]
    [InlineData(19, "dezenove")]
    [InlineData(20, "vinte")]
    [InlineData(21, "vinte e um")]
    [InlineData(99, "noventa e nove")]
    [InlineData(100, "cem")]
    [InlineData(101, "cento e um")]
    [InlineData(150, "cento e cinquenta")]
    [InlineData(200, "duzentos")]
    [InlineData(999, "novecentos e noventa e nove")]
    [InlineData(1000, "mil")]
    [InlineData(1001, "mil e um")]
    [InlineData(1035, "mil e trinta e cinco")]
    [InlineData(1500, "mil e quinhentos")]
    [InlineData(1135, "mil cento e trinta e cinco")]
    [InlineData(2000, "dois mil")]
    [InlineData(1000000, "um milhão")]
    [InlineData(2000000, "dois milhões")]
    [InlineData(1500000, "um milhão e quinhentos mil")]
    public void Word_pt_BR(long value, string expected) =>
        Assert.Equal(expected, H.Word(value, "pt-BR"));

    [Theory]
    [InlineData(-1, "menos um")]
    [InlineData(-15, "menos quinze")]
    [InlineData(-1234, "menos mil duzentos e trinta e quatro")]
    public void Word_handles_negatives(long value, string expected) =>
        Assert.Equal(expected, H.Word(value, "pt-BR"));

    [Theory]
    [InlineData(1, "primeiro")]
    [InlineData(2, "segundo")]
    [InlineData(10, "décimo")]
    [InlineData(11, "décimo primeiro")]
    [InlineData(20, "vigésimo")]
    [InlineData(22, "vigésimo segundo")]
    [InlineData(100, "centésimo")]
    [InlineData(101, "centésimo primeiro")]
    [InlineData(123, "centésimo vigésimo terceiro")]
    [InlineData(1000, "milésimo")]
    [InlineData(1234, "milésimo ducentésimo trigésimo quarto")]
    public void OrdinalWord_pt_BR(long value, string expected) =>
        Assert.Equal(expected, H.OrdinalWord(value, "pt-BR"));

    [Theory]
    [InlineData(1, "1º")]
    [InlineData(22, "22º")]
    [InlineData(100, "100º")]
    public void Ordinal_pt_BR(long value, string expected) =>
        Assert.Equal(expected, H.Ordinal(value, "pt-BR"));

    [Theory]
    [InlineData(1, "janeiro")]
    [InlineData(6, "junho")]
    [InlineData(12, "dezembro")]
    public void Month_pt_BR(int month, string expected) =>
        Assert.Equal(expected, H.Month(month, "pt-BR"));

    [Theory]
    [InlineData(0, "domingo")]
    [InlineData(1, "segunda-feira")]
    [InlineData(5, "sexta-feira")]
    [InlineData(6, "sábado")]
    public void DayOfWeek_pt_BR(int day, string expected) =>
        Assert.Equal(expected, H.DayOfWeek((DayOfWeek)day, "pt-BR"));

    [Fact]
    public void English_still_works() =>
        Assert.Equal("twenty-two", H.Word(22, "en"));

    [Fact]
    public void English_ordinal_works() =>
        Assert.Equal("22nd", H.Ordinal(22, "en"));

    [Fact]
    public void Unknown_culture_falls_back_to_english() =>
        Assert.Equal("twenty-two", H.Word(22, "xx-YY"));
}

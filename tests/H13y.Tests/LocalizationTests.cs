using System.Globalization;
using H13y.Localization;
using H13y.Measures;
using H13y.Tests.Support;
using Xunit;

namespace H13y.Tests;

public class LocalizationTests
{
    private static readonly Dictionary<string, string> PtBr = new()
    {
        ["Unit_kg_Singular"] = "quilograma",
        ["Unit_kg_Plural"] = "quilogramas",
        ["Unit_g_Singular"] = "grama",
        ["Unit_g_Plural"] = "gramas",
        ["Unit_GB_Singular"] = "gigabyte",
        ["Unit_GB_Plural"] = "gigabytes",
        ["Unit_h_Singular"] = "hora",
        ["Unit_h_Plural"] = "horas",
        ["Unit_min_Singular"] = "minuto",
        ["Unit_min_Plural"] = "minutos",
        ["Unit_s_Singular"] = "segundo",
        ["Unit_s_Plural"] = "segundos",
        ["RelativeTime_Now"] = "agora mesmo",
        ["RelativeTime_Moment"] = "daqui a instantes",
        ["RelativeTime_Ago"] = "há {0}",
        ["RelativeTime_In"] = "em {0}",
    };

    private static HumanizeOptions PtBrOptions() =>
        new()
        {
            UnitStyle = UnitStyle.FullName,
            Culture = new CultureInfo("pt-BR"),
            Localizer = new InMemoryLocalizer(PtBr, new CultureInfo("pt-BR")),
        };

    [Fact]
    public void Full_name_falls_back_to_english_when_no_localizer()
    {
        var opts = new HumanizeOptions { UnitStyle = UnitStyle.FullName };
        Assert.Equal("1.5 kilograms", Mass.FromGrams(1500).Humanize(opts));
    }

    [Fact]
    public void Localizer_translates_full_names()
    {
        var opts = PtBrOptions();
        Assert.Equal("1,5 quilogramas", Mass.FromGrams(1500).Humanize(opts));
        Assert.Equal("1 quilograma", Mass.FromKilograms(1).Humanize(opts));
    }

    [Fact]
    public void Localizer_translates_data_size()
    {
        var opts = PtBrOptions();
        Assert.Equal("1 gigabyte", DataSize.FromGigabytes(1).Humanize(opts));
    }

    [Fact]
    public void Localizer_translates_compound_duration_parts()
    {
        var opts = PtBrOptions();
        Assert.Equal("1 hora 1 minuto 1 segundo", Duration.FromSeconds(3661).Humanize(opts));
    }

    [Fact]
    public void Missing_key_falls_back_to_english()
    {
        var opts = PtBrOptions();
        Assert.Equal("1,5 tonnes", Mass.FromTonnes(1.5).Humanize(opts));
    }

    [Fact]
    public void Symbol_style_ignores_localizer()
    {
        var opts = PtBrOptions() with { UnitStyle = UnitStyle.Symbol };
        Assert.Equal("1,5 kg", Mass.FromGrams(1500).Humanize(opts));
    }

    [Fact]
    public void RelativeTime_uses_localized_phrases()
    {
        var now = new DateTime(2026, 10, 9, 12, 0, 0, DateTimeKind.Utc);
        var opts = PtBrOptions();

        Assert.Equal("há 5 minutos", H.RelativeTime(now.AddMinutes(-5), now, opts));
        Assert.Equal("em 2 horas", H.RelativeTime(now.AddHours(2), now, opts));
        Assert.Equal("agora mesmo", H.RelativeTime(now.AddSeconds(-10), now, opts));
    }

    [Fact]
    public void LocalizationKeys_use_expected_format()
    {
        Assert.Equal("Unit_kg_Singular", LocalizationKeys.UnitSingular("kg"));
        Assert.Equal("Unit_GB_Plural", LocalizationKeys.UnitPlural("GB"));
        Assert.Equal("Unit_°C_Singular", LocalizationKeys.UnitSingular("°C"));
    }
}

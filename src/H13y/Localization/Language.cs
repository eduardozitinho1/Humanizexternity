namespace H13y.Localization;

/// <summary>
/// Resolves an <see cref="ILanguageProvider"/> from a culture tag or a
/// <see cref="System.Globalization.CultureInfo"/>.
/// </summary>
public static class Language
{
    private static readonly Dictionary<string, ILanguageProvider> Providers =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new EnglishLanguage(),
            ["en-US"] = new EnglishLanguage(),
            ["en-GB"] = new EnglishLanguage(),
            ["pt"] = new PortugueseLanguage(),
            ["pt-BR"] = new PortugueseLanguage(),
            ["pt-PT"] = new PortugueseLanguage(),
        };

    /// <summary>Gets a provider by culture tag. Falls back to English when unknown.</summary>
    public static ILanguageProvider Get(string culture) =>
        Providers.TryGetValue(culture, out var p) ? p : Providers["en"];

    /// <summary>Registers a custom provider for a culture tag.</summary>
    public static void Register(ILanguageProvider provider) =>
        Providers[provider.Culture] = provider;
}

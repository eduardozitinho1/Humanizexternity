namespace H13y.Localization;

/// <summary>
/// A language-specific provider for number words, ordinals, and calendar names.
/// </summary>
public interface ILanguageProvider
{
    /// <summary>BCP-47 tag of this language, e.g. <c>"en"</c> or <c>"pt-BR"</c>.</summary>
    string Culture { get; }

    /// <summary>Spells an integer as words in this language.</summary>
    string Word(long value);

    /// <summary>Formats an ordinal suffix or abbreviation, e.g. "22nd" or "22º".</summary>
    string Ordinal(long value);

    /// <summary>Spells an ordinal as words, e.g. "twenty-second" or "vigésimo segundo".</summary>
    string OrdinalWord(long value);

    /// <summary>Name of a month (1..12).</summary>
    string Month(int month);

    /// <summary>Name of a day of the week.</summary>
    string DayOfWeek(DayOfWeek day);
}

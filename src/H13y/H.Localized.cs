using System.Globalization;
using H13y.Localization;

namespace H13y;

public static partial class H
{
    /// <summary>Spells a number as words in the given culture.</summary>
    public static string Word(long value, string culture) =>
        Language.Get(culture).Word(value);

    /// <summary>Spells a number as words in the given culture.</summary>
    public static string Word(long value, CultureInfo culture) =>
        Word(value, culture.Name);

    /// <summary>Formats an ordinal in the given culture ("22nd" / "22º").</summary>
    public static string Ordinal(long value, string culture) =>
        Language.Get(culture).Ordinal(value);

    /// <summary>Spells an ordinal as words in the given culture.</summary>
    public static string OrdinalWord(long value, string culture) =>
        Language.Get(culture).OrdinalWord(value);

    /// <summary>Returns a month name in the given culture.</summary>
    public static string Month(int month, string culture) =>
        Language.Get(culture).Month(month);

    /// <summary>Returns a day of week name in the given culture.</summary>
    public static string DayOfWeek(DayOfWeek day, string culture) =>
        Language.Get(culture).DayOfWeek(day);
}

using System.Text;

namespace H13y;

public static partial class H
{
    /// <summary>Converts text to snake_case.</summary>
    public static string SnakeCase(string text) => CaseConverter.ToSnakeCase(text);

    /// <summary>Converts text to kebab-case.</summary>
    public static string KebabCase(string text) => CaseConverter.ToKebabCase(text);

    /// <summary>Converts text to camelCase.</summary>
    public static string CamelCase(string text) => CaseConverter.ToCamelCase(text);

    /// <summary>Converts text to PascalCase.</summary>
    public static string PascalCase(string text) => CaseConverter.ToPascalCase(text);

    /// <summary>Converts text to Title Case.</summary>
    public static string TitleCase(string text) => CaseConverter.ToTitleCase(text);

    /// <summary>Converts text to SCREAMING_SNAKE_CASE.</summary>
    public static string UpperSnakeCase(string text) => CaseConverter.ToUpperSnakeCase(text);

    /// <summary>Converts an identifier into a readable sentence.</summary>
    public static string Humanize(string text) => CaseConverter.ToHumanized(text);
}

internal static class CaseConverter
{
    public static string ToSnakeCase(string text) =>
        string.Join('_', SplitWords(text).Select(w => w.ToLowerInvariant()));

    public static string ToKebabCase(string text) =>
        string.Join('-', SplitWords(text).Select(w => w.ToLowerInvariant()));

    public static string ToCamelCase(string text)
    {
        var words = SplitWords(text);
        if (words.Count == 0)
            return string.Empty;
        var sb = new StringBuilder(words[0].ToLowerInvariant());
        for (var i = 1; i < words.Count; i++)
            sb.Append(Capitalize(words[i]));
        return sb.ToString();
    }

    public static string ToPascalCase(string text) =>
        string.Concat(SplitWords(text).Select(Capitalize));

    public static string ToTitleCase(string text) =>
        string.Join(' ', SplitWords(text).Select(Capitalize));

    public static string ToUpperSnakeCase(string text) =>
        string.Join('_', SplitWords(text).Select(w => w.ToUpperInvariant()));

    public static string ToHumanized(string text)
    {
        var words = SplitWords(text);
        if (words.Count == 0)
            return string.Empty;
        return char.ToUpperInvariant(words[0][0]) + words[0][1..].ToLowerInvariant()
            + (words.Count > 1
                ? " " + string.Join(' ', words.Skip(1).Select(w => w.ToLowerInvariant()))
                : string.Empty);
    }

    private static List<string> SplitWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var result = new List<string>();
        var current = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c is '_' or '-' or ' ' or '.')
            {
                if (current.Length > 0)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                continue;
            }

            if (i > 0 && char.IsUpper(c) && current.Length > 0)
            {
                var prev = text[i - 1];
                var next = i + 1 < text.Length ? text[i + 1] : '\0';
                var splitHere = !char.IsUpper(prev)
                    || (char.IsUpper(prev) && next != '\0' && !char.IsUpper(next));
                if (splitHere)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
            }

            current.Append(c);
        }

        if (current.Length > 0)
            result.Add(current.ToString());

        return result;
    }

    private static string Capitalize(string word) =>
        word.Length == 0
            ? word
            : char.ToUpperInvariant(word[0]) + word[1..].ToLowerInvariant();
}

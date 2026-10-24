using System.Text.RegularExpressions;

namespace H13y;

public static partial class H
{
    /// <summary>Returns the plural form of an English noun.</summary>
    public static string Pluralize(string word) => Inflector.Pluralize(word);

    /// <summary>Returns the singular form of an English noun.</summary>
    public static string Singularize(string word) => Inflector.Singularize(word);

    /// <summary>Returns true when <paramref name="word"/> is already plural.</summary>
    public static bool IsPlural(string word) => Inflector.IsPlural(word);

    /// <summary>Returns true when <paramref name="word"/> is already singular.</summary>
    public static bool IsSingular(string word) => Inflector.IsSingular(word);
}

internal static class Inflector
{
    private static readonly HashSet<string> Uncountables = new(StringComparer.OrdinalIgnoreCase)
    {
        "advice", "aircraft", "bison", "cod", "deer", "equipment", "fish", "gold",
        "information", "jeans", "means", "moose", "news", "offspring", "police",
        "rice", "salmon", "series", "sheep", "shrimp", "spacecraft", "species",
        "swine", "trout", "tuna", "money",
    };

    private static readonly Dictionary<string, string> Irregular = new(StringComparer.OrdinalIgnoreCase)
    {
        ["person"] = "people", ["man"] = "men", ["woman"] = "women",
        ["child"] = "children", ["tooth"] = "teeth", ["foot"] = "feet",
        ["goose"] = "geese", ["mouse"] = "mice", ["louse"] = "lice",
        ["ox"] = "oxen", ["die"] = "dice",
        ["criterion"] = "criteria", ["phenomenon"] = "phenomena",
        ["datum"] = "data", ["medium"] = "media", ["bacterium"] = "bacteria",
        ["curriculum"] = "curricula", ["cactus"] = "cacti", ["focus"] = "foci",
        ["fungus"] = "fungi", ["nucleus"] = "nuclei", ["syllabus"] = "syllabi",
        ["analysis"] = "analyses", ["basis"] = "bases", ["crisis"] = "crises",
        ["thesis"] = "theses", ["axis"] = "axes", ["index"] = "indices",
        ["matrix"] = "matrices", ["vertex"] = "vertices", ["appendix"] = "appendices",
        ["alumnus"] = "alumni", ["alumna"] = "alumnae",
        ["formula"] = "formulae", ["antenna"] = "antennae",
        ["quiz"] = "quizzes",
    };

    private static readonly Dictionary<string, string> ReverseIrregular =
        Irregular.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.OrdinalIgnoreCase);

    private static readonly (Regex Pattern, string Replacement)[] PluralRules =
    [
        (new Regex(@"(?i)(quiz)$"), "${1}zes"),
        (new Regex(@"(?i)(matr|vert|ind)(ix|ex)$"), "${1}ices"),
        (new Regex(@"(?i)(x|ch|ss|sh)$"), "$1es"),
        (new Regex(@"(?i)([^aeiouy]|qu)y$"), "${1}ies"),
        (new Regex(@"(?i)(hive)$"), "$1s"),
        (new Regex(@"(?i)(?:([^f])fe|([lr])f)$"), "$1$2ves"),
        (new Regex(@"(?i)sis$"), "ses"),
        (new Regex(@"(?i)([ti])um$"), "$1a"),
        (new Regex(@"(?i)(buffal|tomat)o$"), "${1}oes"),
        (new Regex(@"(?i)(bu)s$"), "${1}ses"),
        (new Regex(@"(?i)(alias|status)$"), "${1}es"),
        (new Regex(@"(?i)(octop|vir)us$"), "${1}i"),
        (new Regex(@"(?i)^(ax|test)is$"), "${1}es"),
        (new Regex(@"(?i)s$"), "s"),
        (new Regex(@"(?i)$"), "s"),
    ];

    private static readonly (Regex Pattern, string Replacement)[] SingularRules =
    [
        (new Regex(@"(?i)(quiz)zes$"), "$1"),
        (new Regex(@"(?i)(matr)ices$"), "${1}ix"),
        (new Regex(@"(?i)(vert|ind)ices$"), "${1}ex"),
        (new Regex(@"(?i)^(ox)en$"), "$1"),
        (new Regex(@"(?i)(alias|status)(es)?$"), "$1"),
        (new Regex(@"(?i)(octop|vir)i$"), "${1}us"),
        (new Regex(@"(?i)(cris|ax|test)es$"), "${1}is"),
        (new Regex(@"(?i)(shoe)s$"), "$1"),
        (new Regex(@"(?i)(o)es$"), "$1"),
        (new Regex(@"(?i)(bus)(es)?$"), "$1"),
        (new Regex(@"(?i)([m|l])ice$"), "${1}ouse"),
        (new Regex(@"(?i)(x|ch|ss|sh)es$"), "$1"),
        (new Regex(@"(?i)(m)ovies$"), "${1}ovie"),
        (new Regex(@"(?i)(s)eries$"), "${1}eries"),
        (new Regex(@"(?i)([^aeiouy]|qu)ies$"), "${1}y"),
        (new Regex(@"(?i)([lr])ves$"), "${1}f"),
        (new Regex(@"(?i)(tive)s$"), "$1"),
        (new Regex(@"(?i)(hive)s$"), "$1"),
        (new Regex(@"(?i)([^f])ves$"), "${1}fe"),
        (new Regex(@"(?i)(^analy)ses$"), "${1}sis"),
        (new Regex(@"(?i)((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$"), "${1}${2}sis"),
        (new Regex(@"(?i)([ti])a$"), "${1}um"),
        (new Regex(@"(?i)(n)ews$"), "${1}ews"),
        (new Regex(@"(?i)(ss)$"), "$1"),
        (new Regex(@"(?i)s$"), ""),
    ];

    public static string Pluralize(string word)
    {
        if (string.IsNullOrEmpty(word))
            return word;

        var lower = word.ToLowerInvariant();

        if (Uncountables.Contains(lower))
            return word;

        if (Irregular.TryGetValue(lower, out var irregular))
            return MatchCase(word, irregular);

        if (ReverseIrregular.ContainsKey(lower))
            return word;

        foreach (var (pattern, replacement) in PluralRules)
        {
            if (pattern.IsMatch(word))
                return pattern.Replace(word, replacement);
        }

        return word + "s";
    }

    public static string Singularize(string word)
    {
        if (string.IsNullOrEmpty(word))
            return word;

        var lower = word.ToLowerInvariant();

        if (Uncountables.Contains(lower))
            return word;

        if (ReverseIrregular.TryGetValue(lower, out var irregular))
            return MatchCase(word, irregular);

        if (Irregular.ContainsKey(lower))
            return word;

        foreach (var (pattern, replacement) in SingularRules)
        {
            if (pattern.IsMatch(word))
                return pattern.Replace(word, replacement);
        }

        return word;
    }

    public static bool IsPlural(string word) =>
        !string.IsNullOrEmpty(word)
        && !Uncountables.Contains(word)
        && Pluralize(Singularize(word)).Equals(word, StringComparison.OrdinalIgnoreCase);

    public static bool IsSingular(string word) =>
        !string.IsNullOrEmpty(word)
        && !Uncountables.Contains(word)
        && Singularize(Pluralize(word)).Equals(word, StringComparison.OrdinalIgnoreCase);

    private static string MatchCase(string source, string replacement)
    {
        if (source.Length == 0)
            return replacement;
        if (source.All(char.IsUpper))
            return replacement.ToUpperInvariant();
        if (char.IsUpper(source[0]))
            return char.ToUpperInvariant(replacement[0]) + replacement[1..];
        return replacement;
    }
}

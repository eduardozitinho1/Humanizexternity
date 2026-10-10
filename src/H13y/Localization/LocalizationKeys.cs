namespace H13y.Localization;

/// <summary>
/// Key naming conventions used when resolving localized strings through
/// <see cref="Microsoft.Extensions.Localization.IStringLocalizer"/>.
/// </summary>
/// <remarks>
/// Applications provide a localizer (typically built from a <c>ResourceManager</c>
/// pointing at their own <c>.resx</c> files) and register entries using these keys:
///
/// <list type="bullet">
///   <item><description><c>Unit_{symbol}_Singular</c> — e.g. <c>Unit_kg_Singular</c> = "quilograma".</description></item>
///   <item><description><c>Unit_{symbol}_Plural</c> — e.g. <c>Unit_kg_Plural</c> = "quilogramas".</description></item>
///   <item><description><c>RelativeTime_Now</c>, <c>RelativeTime_Moment</c>, <c>RelativeTime_Ago</c>, <c>RelativeTime_In</c> — phrases.</description></item>
/// </list>
///
/// Missing keys fall back to the built-in English strings, so applications can
/// localize incrementally without breaking anything.
/// </remarks>
public static class LocalizationKeys
{
    /// <summary>Key for the singular display name of a unit symbol.</summary>
    public static string UnitSingular(string symbol) => $"Unit_{symbol}_Singular";

    /// <summary>Key for the plural display name of a unit symbol.</summary>
    public static string UnitPlural(string symbol) => $"Unit_{symbol}_Plural";

    /// <summary>Phrase for "just now".</summary>
    public const string RelativeTimeNow = "RelativeTime_Now";

    /// <summary>Phrase for "in a moment".</summary>
    public const string RelativeTimeMoment = "RelativeTime_Moment";

    /// <summary>Template for "{0} ago" — receives the duration phrase as argument 0.</summary>
    public const string RelativeTimeAgo = "RelativeTime_Ago";

    /// <summary>Template for "in {0}" — receives the duration phrase as argument 0.</summary>
    public const string RelativeTimeIn = "RelativeTime_In";

    /// <summary>Prefix for "about".</summary>
    public const string ApproximateAbout = "Approximate_About";

    /// <summary>Prefix for "roughly".</summary>
    public const string ApproximateRoughly = "Approximate_Roughly";

    /// <summary>Prefix for "approximately".</summary>
    public const string ApproximateApproximately = "Approximate_Approximately";
}

using System.Globalization;
using Microsoft.Extensions.Localization;

namespace H13y.Tests.Support;

internal sealed class InMemoryLocalizer : IStringLocalizer
{
    private readonly Dictionary<string, string> _strings;

    public InMemoryLocalizer(Dictionary<string, string> strings, CultureInfo? culture = null)
    {
        _strings = strings;
        Culture = culture ?? CultureInfo.InvariantCulture;
    }

    public CultureInfo Culture { get; }

    public LocalizedString this[string name]
    {
        get
        {
            var found = _strings.TryGetValue(name, out var value);
            return new LocalizedString(name, value ?? name, resourceNotFound: !found);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var found = _strings.TryGetValue(name, out var value);
            var formatted = found ? string.Format(Culture, value!, arguments) : name;
            return new LocalizedString(name, formatted, resourceNotFound: !found);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
        _strings.Select(kv => new LocalizedString(kv.Key, kv.Value, resourceNotFound: false));
}

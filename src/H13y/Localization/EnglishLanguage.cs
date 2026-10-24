namespace H13y.Localization;

internal sealed class EnglishLanguage : ILanguageProvider
{
    public string Culture => "en";

    private static readonly string[] Months =
    [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December",
    ];

    private static readonly string[] Days =
    [
        "Sunday", "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday",
    ];

    public string Word(long value) => WordFormatter.Format(value);

    public string Ordinal(long value) => OrdinalFormatter.Format(value);

    public string OrdinalWord(long value) => OrdinalFormatter.Word(value);

    public string Month(int month) =>
        month is < 1 or > 12
            ? throw new ArgumentOutOfRangeException(nameof(month), month, "Month must be 1..12.")
            : Months[month - 1];

    public string DayOfWeek(DayOfWeek day) => Days[(int)day];
}

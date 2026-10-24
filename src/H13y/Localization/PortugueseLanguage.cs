namespace H13y.Localization;

internal sealed class PortugueseLanguage : ILanguageProvider
{
    public string Culture => "pt-BR";

    private static readonly string[] Months =
    [
        "janeiro", "fevereiro", "março", "abril", "maio", "junho",
        "julho", "agosto", "setembro", "outubro", "novembro", "dezembro",
    ];

    private static readonly string[] Days =
    [
        "domingo", "segunda-feira", "terça-feira", "quarta-feira",
        "quinta-feira", "sexta-feira", "sábado",
    ];

    private static readonly string[] Units =
    [
        "zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete",
        "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze",
        "dezesseis", "dezessete", "dezoito", "dezenove",
    ];

    private static readonly string[] Tens =
    [
        "", "", "vinte", "trinta", "quarenta", "cinquenta",
        "sessenta", "setenta", "oitenta", "noventa",
    ];

    private static readonly string[] Hundreds =
    [
        "", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos",
        "seiscentos", "setecentos", "oitocentos", "novecentos",
    ];

    private static readonly string[] UnitsOrdinal =
    [
        "zero", "primeiro", "segundo", "terceiro", "quarto", "quinto",
        "sexto", "sétimo", "oitavo", "nono",
    ];

    private static readonly string[] TensOrdinal =
    [
        "", "décimo", "vigésimo", "trigésimo", "quadragésimo",
        "quinquagésimo", "sexagésimo", "septuagésimo", "octogésimo", "nonagésimo",
    ];

    private static readonly string[] HundredsOrdinal =
    [
        "", "centésimo", "ducentésimo", "tricentésimo", "quadringentésimo",
        "quingentésimo", "sexcentésimo", "septingentésimo",
        "octingentésimo", "nongentésimo",
    ];

    public string Word(long value)
    {
        if (value == 0)
            return "zero";

        if (value < 0)
            return "menos " + Word(-value);

        if (value >= 1_000_000_000_000)
            return Word(value / 1_000_000_000_000)
                + (value / 1_000_000_000_000 == 1 ? " trilhão" : " trilhões")
                + BuildRest(value % 1_000_000_000_000);

        if (value >= 1_000_000_000)
            return Word(value / 1_000_000_000)
                + (value / 1_000_000_000 == 1 ? " bilhão" : " bilhões")
                + BuildRest(value % 1_000_000_000);

        if (value >= 1_000_000)
            return Word(value / 1_000_000)
                + (value / 1_000_000 == 1 ? " milhão" : " milhões")
                + BuildRest(value % 1_000_000);

        if (value >= 1000)
            return (value / 1000 == 1 ? "mil" : Word(value / 1000) + " mil")
                + BuildRest(value % 1000);

        return WordBelowThousand(value);
    }

    private string BuildRest(long rest)
    {
        if (rest == 0)
            return string.Empty;
        return (rest < 100 || rest % 100 == 0 ? " e " : " ") + Word(rest);
    }

    private static string WordBelowThousand(long value)
    {
        if (value == 0)
            return string.Empty;

        if (value < 20)
            return Units[value];

        if (value < 100)
        {
            var tens = value / 10;
            var units = value % 10;
            return units == 0 ? Tens[tens] : Tens[tens] + " e " + Units[units];
        }

        if (value == 100)
            return "cem";

        var hundreds = value / 100;
        var rest = value % 100;
        return rest == 0
            ? Hundreds[hundreds]
            : Hundreds[hundreds] + " e " + WordBelowThousand(rest);
    }

    public string Ordinal(long value)
    {
        if (value < 0)
            return "−" + Math.Abs(value) + "º";
        return value + "º";
    }

    public string OrdinalWord(long value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Ordinal words require non-negative values.");
        if (value == 0)
            return "zero";
        return OrdinalWordRecursive(value).Trim();
    }

    private string OrdinalWordRecursive(long value)
    {
        var parts = new List<string>();

        if (value >= 1_000_000_000)
        {
            var billions = value / 1_000_000_000;
            parts.Add(Word(billions) + (billions == 1 ? " bilionésimo" : " bilionésimos"));
            value %= 1_000_000_000;
        }

        if (value >= 1_000_000)
        {
            var millions = value / 1_000_000;
            parts.Add(Word(millions) + (millions == 1 ? " milionésimo" : " milionésimos"));
            value %= 1_000_000;
        }

        if (value >= 1000)
        {
            var thousands = value / 1000;
            parts.Add(thousands == 1 ? "milésimo" : Word(thousands) + " milésimos");
            value %= 1000;
        }

        if (value >= 100)
        {
            var hundreds = value / 100;
            parts.Add(HundredsOrdinal[hundreds]);
            value %= 100;
        }

        if (value >= 10)
        {
            var tens = value / 10;
            parts.Add(TensOrdinal[tens]);
            value %= 10;
        }

        if (value > 0)
            parts.Add(UnitsOrdinal[value]);

        return string.Join(' ', parts.Where(p => !string.IsNullOrEmpty(p)));
    }

    public string Month(int month) =>
        month is < 1 or > 12
            ? throw new ArgumentOutOfRangeException(nameof(month), month, "Month must be 1..12.")
            : Months[month - 1];

    public string DayOfWeek(DayOfWeek day) => Days[(int)day];
}

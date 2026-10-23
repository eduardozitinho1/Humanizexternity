# Humanizexternity

A C# library for turning raw numeric values into readable strings. `1024 MB` becomes `"1 GB"`, `1500 g` becomes `"1.5 kg"`, `3661 s` becomes `"1 h 1 min 1 s"`.

## Install

    dotnet add package Humanizexternity

## Quick start

    using H13y;

    H.Bytes(1073741824);          // "1 GB"
    H.Grams(1500);                // "1.5 kg"
    H.Seconds(3661);              // "1 h 1 min 1 s"
    H.Parse("1.5 kg").ToBase();   // 1500

## Links

- [GitHub repository](https://github.com/eduardozitinho1/Humanizexternity)
- [NuGet package](https://www.nuget.org/packages/Humanizexternity)
- [Author portfolio](https://eduardoweb.us.ci)

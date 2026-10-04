using BenchmarkDotNet.Attributes;
using H13y;
using H13y.Measures;

namespace H13y.Benchmarks;

[MemoryDiagnoser]
public class HumanizeBenchmarks
{
    [Benchmark]
    public string Bytes_1GB() => H.Bytes(1073741824);

    [Benchmark]
    public string Grams_1_5kg() => H.Grams(1500);

    [Benchmark]
    public string Meters_5mm() => H.Meters(0.005);

    [Benchmark]
    public string Seconds_3661() => H.Seconds(3661);

    [Benchmark]
    public string Kph_100() => H.KilometersPerHour(100);

    [Benchmark]
    public string Kwh_1() => H.KilowattHours(1);

    [Benchmark]
    public string Degrees_45() => H.Degrees(45);

    [Benchmark]
    public double Convert_kg_to_g() => H.Convert(1.5, Units.Mass.Kilogram, Units.Mass.Gram);
}

[MemoryDiagnoser]
public class ParseBenchmarks
{
    [Benchmark]
    public Measure Parse_simple() => H.Parse("1.5 kg");

    [Benchmark]
    public Measure Parse_with_slash() => H.Parse("100 km/h");

    [Benchmark]
    public Measure Parse_with_comma() => H.Parse("1,5 GB");

    [Benchmark]
    public Measure Parse_compound_duration() => H.Parse("1h30min");

    [Benchmark]
    public Measure Parse_colon_duration() => H.Parse("1:30:45");
}

using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace dotnet_benchmarks_scratch;

[MemoryDiagnoser]
public class MyBenchmarks
{
    [Params(1000, 10000)]
    public int N;

    [Params(2)]
    public int GetLengthCount;

    private List<string> paths;

    public static IEnumerable<string> GeneratePaths(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return $"c:\\my\\sample\\file\\path\\{i}.txt";
        }
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.paths = GeneratePaths(this.N).ToList();
    }

    [Benchmark]
    public void BuildHashSetAndGetCount()
    {
        var hashset = new HashSet<string>(this.paths);

        for (var i = 0; i < this.GetLengthCount; i++)
        {
            hashset.Count();
        }
    }

    [Benchmark]
    public void BuildImmutableArrayThenHashSetAndGetLength()
    {
        var array = this.paths.ToImmutableArray();
        var hashset = new HashSet<string>(array);

        for (var i = 0; i < this.GetLengthCount; i++)
        {
            var _ = array.Length;
        }
    }
}

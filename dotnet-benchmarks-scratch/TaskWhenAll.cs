using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace dotnet_benchmarks_scratch;

[MemoryDiagnoser]
public class TaskWhenAll
{
    [Params(1000, 10000)]
    public int N;

    public static IEnumerable<Task<int>> GenerateTasks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Task.FromResult(i);
        }
    }

    [Benchmark]
    public async Task WhenAllIEnumerable()
    {
        var tasks = GenerateTasks(this.N);
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task WhenAllList()
    {
        var tasks = GenerateTasks(this.N).ToList();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task WhenAllArray()
    {
        var tasks = GenerateTasks(this.N).ToArray();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task WhenAllImmutableArray()
    {
        var tasks = GenerateTasks(this.N).ToImmutableArray();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task WhenAllArrayAsIReadOnlyList()
    {
        var tasks = GenerateTasks(this.N).ToArray() as IReadOnlyList<Task<int>>;
        await Task.WhenAll(tasks);
    }
}

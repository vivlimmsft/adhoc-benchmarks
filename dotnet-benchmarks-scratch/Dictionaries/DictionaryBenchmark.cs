using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace dotnet_benchmarks_scratch.Dictionaries;

[MemoryDiagnoser]
public class DictionaryBenchmark
{
    [Params(4000)]
    public int N;

    [Params(1, 2, 4)]
    public int Threads;

    private string[] keys = [];
    private string[] values = [];
    private int[][] segments = [];

    public static string ValueForKey(string key)
    {
        return $"value for {key}";
    }

    public static IEnumerable<string> GenerateKeys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return $"key_{i}";
        }
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.keys = GenerateKeys(this.N).ToArray();
        this.values = this.keys.Select(ValueForKey).ToArray();
        this.segments = Enumerable.Range(0, N).Chunk(N / Threads).Take(Threads).ToArray();

        int nFromSegments = segments.Select(s => s.Length).Sum();
        if (nFromSegments != N)
        {
            throw new Exception($"Segments do not sum to N (they sum to {nFromSegments})");
        }
    }

    [Benchmark]
    public async Task RegularDictionary()
    {
        var dictionary = new Dictionary<string, string>();

        await ActOnDictionary(dictionary);
    }

    [Benchmark]
    public async Task LockingDictionary()
    {
        var dictionary = new LockingDictionary<string, string>();
        await ActOnDictionary(dictionary);
    }

    [Benchmark]
    public async Task ConcurrentDictionary()
    {
        var dictionary = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
        await ActOnDictionary(dictionary);
    }

    private async Task ActOnDictionary(IDictionary<string, string> dictionary)
    {
        Task[] tasks = segments.Select((segment) => Task.Run(() => ActOnDictionaryThread(dictionary, segment))).ToArray();
        await Task.WhenAll(tasks);
    }

    private void ActOnDictionaryThread(IDictionary<string, string> dictionary, int[] indices)
    {
        foreach (var i in indices)
        {
            dictionary[this.keys[i]] = this.values[i];
        }
        foreach (var i in indices)
        {
            var _ = dictionary[this.keys[i]];
        }
    }
}

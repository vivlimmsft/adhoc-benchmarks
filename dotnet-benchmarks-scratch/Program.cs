using BenchmarkDotNet.Running;
using dotnet_benchmarks_scratch;
using dotnet_benchmarks_scratch.Dictionaries;

//BenchmarkRunner.Run<AsyncInitializingProperties>();
//BenchmarkRunner.Run<MyBenchmarks>();
//BenchmarkRunner.Run<TaskWhenAll>();
BenchmarkRunner.Run<DictionaryBenchmark>();
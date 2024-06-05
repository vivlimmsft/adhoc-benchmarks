using BenchmarkDotNet.Running;
using dotnet_benchmarks_scratch;

BenchmarkRunner.Run<AsyncInitializingProperties>();
BenchmarkRunner.Run<MyBenchmarks>();
BenchmarkRunner.Run<TaskWhenAll>();

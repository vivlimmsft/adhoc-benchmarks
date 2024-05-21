using BenchmarkDotNet.Running;
using dotnet_benchmarks_scratch;

BenchmarkRunner.Run<MyBenchmarks>();
BenchmarkRunner.Run<TaskWhenAll>();

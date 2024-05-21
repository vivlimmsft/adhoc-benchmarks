using BenchmarkDotNet.Running;
using dotnet_benchmarks_scratch;

var summary = BenchmarkRunner.Run<MyBenchmarks>();

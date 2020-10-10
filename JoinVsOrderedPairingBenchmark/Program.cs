using BenchmarkDotNet.Running;
using JoinVsOrderedPairingBenchmark.Benchmarks;
using System;

namespace JoinVsOrderedPairingBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<VsJoin>();
        }
    }
}

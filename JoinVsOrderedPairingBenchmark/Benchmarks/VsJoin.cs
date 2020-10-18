using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Engines;
using JoinVsOrderedPairing.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairingBenchmark.Benchmarks
{
    [RPlotExporter]
    //[InliningDiagnoser(false, true)]
    [MemoryDiagnoser]
    [CsvMeasurementsExporter]
    public class VsJoin
    {
        private IEnumerable<int> _left;
        private IEnumerable<int> _right;
        private Func<int, int> _leftKeySelector;
        private Func<int, int> _rightKeySelector;
        private Func<int, int, (int, int)> _resultKeySelector;

        private readonly Consumer _consumer = new Consumer();

        //[Params(1 << 13, 1 << 16/*, 1 << 19*/)]
        //[Params(100, 1000, 10000)]
        [Params(50000)]
        public int Length;

        [GlobalSetup]
        public void Setup()
        {
            _left = Enumerable.Range(0, Length).ToArray();
            _right = Enumerable.Range(0, Length).ToArray();
            _leftKeySelector = x => x;
            _rightKeySelector = x => x;
            _resultKeySelector = (l, r) => (l, r);
        }

        [Benchmark]
        public void OrderedPairing() => _left
            .PairSelect(_right, _leftKeySelector, _rightKeySelector, _resultKeySelector)
            .Consume(_consumer);

        [Benchmark]
        public void JoinPairing() => _left
            .JoinSelect(_right, _leftKeySelector, _rightKeySelector, _resultKeySelector)
            .Consume(_consumer);

        //[Benchmark(Baseline = true)]
        [Benchmark]
        public void NaivePairing() => _left
            .PairSelectNaive(_right, _leftKeySelector, _rightKeySelector, _resultKeySelector)
            .Consume(_consumer);

        [Benchmark]
        public void ManuallyInlined() => _left
            .PairSelectManuallyInlined(_right, _leftKeySelector, _rightKeySelector, _resultKeySelector)
            .Consume(_consumer);
    }
}

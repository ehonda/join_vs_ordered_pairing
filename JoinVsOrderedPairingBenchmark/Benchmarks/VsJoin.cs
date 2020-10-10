using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JoinVsOrderedPairing.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairingBenchmark.Benchmarks
{
    [RPlotExporter]
    public class VsJoin
    {
        private IEnumerable<int> _left;
        private IEnumerable<int> _right;
        private Func<int, int> _leftKeySelector;
        private Func<int, int> _rightKeySelector;
        private Func<int, int, (int, int)> _resultKeySelector;

        private readonly Consumer _consumer = new Consumer();


        [Params(10000, 100000)]
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

        [Benchmark(Baseline = true)]
        public void JoinPairing() => _left
            .JoinSelect(_right, _leftKeySelector, _rightKeySelector, _resultKeySelector)
            .Consume(_consumer);
    }
}

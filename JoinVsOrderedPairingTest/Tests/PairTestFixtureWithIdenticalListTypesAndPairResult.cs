using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class PairTestFixtureWithIdenticalListTypesAndPairResult<T, Key>
        : PairTestFixture<T, T, Key, (T, T)>
        where Key : IComparable<Key>
    {
        private readonly Func<(T, T), (T, T), bool> _pairComparator;

        public PairTestFixtureWithIdenticalListTypesAndPairResult(
            Func<T, Key> keySelector,
            Func<
                IEnumerable<T>,
                IEnumerable<T>,
                Func<T, Key>,
                Func<T, Key>,
                Func<T, T, (T, T)>,
                IEnumerable<(T, T)>> pairSelect)
            : base(keySelector, keySelector, (l, r) => (l, r), pairSelect)
        {
            _pairComparator = (p, q) =>
                keySelector(p.Item1).IsEqualTo(keySelector(q.Item1))
                && keySelector(p.Item2).IsEqualTo(keySelector(q.Item2));
        }

        protected void ExpectNumberOfPairs(int expectedCount)
            => ExpectNumberOfResults(expectedCount);

        protected void ExpectExactlyOnePairOf((T, T) expectedPair)
            => ExpectExactlyOneResultOf(expectedPair, _pairComparator);

        protected void ExpectExactlyNPairsOf(int expectedCount, (T, T) expectedPair)
            => ExpectExactlyNResultsOf(expectedCount, expectedPair, _pairComparator);

        protected void TestWithSymmetricalSetups(Action<List<T>, List<T>> test)
        {
            SetUp();
            test(_left, _right);
            SetUp();
            test(_right, _left);
        }
    }
}

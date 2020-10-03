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
        private readonly Func<T, Key> _keySelector;
        private readonly Func<(T, T), (T, T), bool> _resultComparator;

        public PairTestFixtureWithIdenticalListTypesAndPairResult(
            Func<T, Key> keySelector)
            : base(keySelector, keySelector, (l, r) => (l, r))
        {
            _keySelector = keySelector;
            _resultComparator = (p, q) =>
                _keySelector(p.Item1).IsEqualTo(_keySelector(q.Item1))
                && _keySelector(p.Item2).IsEqualTo(_keySelector(q.Item2));
        }

        protected void ExpectNumberOfPairs(int expectedCount)
            => ExpectNumberOfResults(expectedCount);

        protected void ExpectExactlyOnePairOf((T, T) expectedPair)
            => ExpectExactlyOneResultOf(expectedPair, _resultComparator);

        protected void ExpectExactlyNPairsOf(int expectedCount, (T, T) expectedPair)
            => ExpectExactlyNResultsOf(expectedCount, expectedPair, _resultComparator);

        protected void TestWithSymmetricalSetups(Action<List<T>, List<T>> test)
        {
            SetUp();
            test(_left, _right);
            SetUp();
            test(_right, _left);
        }
    }
}

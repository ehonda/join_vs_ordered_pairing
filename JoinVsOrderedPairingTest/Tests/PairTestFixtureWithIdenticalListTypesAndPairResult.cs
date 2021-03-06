﻿using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

// The type of our pair selector function. We would really like to do
//      using PairSelector<T, Key> = ...
// but this is unfortunately not legal C# since type alias using directives
// can't have type parmeters, see 
//      https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces#using-directives
//
//  Func<
//      IEnumerable<T>,
//      IEnumerable<T>,
//      Func<T, Key>,
//      Func<T, Key>,
//      Func<T, T, (T, T)>,
//      IEnumerable<(T, T)>>

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class PairTestFixtureWithIdenticalListTypesAndPairResult<T, Key>
        : PairTestFixture<T, T, Key, (T, T)>
        where Key : IComparable<Key>
    {
        private readonly Func<(T, T), (T, T), bool> _pairComparator;

        public PairTestFixtureWithIdenticalListTypesAndPairResult(
            Func<T, Key> keySelector)
            : base(keySelector, keySelector, (l, r) => (l, r))
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

        protected void TestWithImplementationAndWithSymmetricalSetups(
            Func<
                IEnumerable<T>,
                IEnumerable<T>,
                Func<T, Key>,
                Func<T, Key>,
                Func<T, T, (T, T)>,
                IEnumerable<(T, T)>> implementation,
            Action<List<T>, List<T>> test)
            => TestWithPairSelect(implementation, () =>
                {
                    SetUp();
                    test(_left, _right);
                    SetUp();
                    test(_right, _left);
                });
    }
}

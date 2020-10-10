﻿using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JoinVsOrderedPairingTest.Tests
{
    public static class Functions
    {
        public static Func<int, int> Identity() => x => x;
        public static Func<(int, int), int> First() => pair => pair.Item1;
    }

    //[TestFixture(typeof(int), typeof(int), typeof(int), typeof(int), 
    //    new Func<int, int>(x => x),
    //    new Func<int, int>(x => x),
    //    new Func<int, int>((l, r) => l)]
    //[TestFixture(Functions.Identity(), Functions.Identity(), Functions.First())]
    public class PairTestFixture<Left, Right, Key, Result>
        where Key : IComparable<Key>
    {
        protected List<Left> _left;
        protected List<Right> _right;
        private readonly Func<Left, Key> _leftKeySelector;
        private readonly Func<Right, Key> _rightKeySelector;
        private readonly Func<Left, Right, Result> _resultSelector;

        public PairTestFixture(
            Func<Left, Key> leftKeySelector, 
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
        {
            _leftKeySelector = leftKeySelector;
            _rightKeySelector = rightKeySelector;
            _resultSelector = resultSelector;
        }

        protected IEnumerable<Result> PairSelectResult() => 
            _left.PairSelect(_right, _leftKeySelector, _rightKeySelector, _resultSelector);

        protected void ExpectNumberOfResults(int expectedCount)
            => Assert.That(PairSelectResult(), Has.Exactly(expectedCount).Items);

        protected void ExpectExactlyOneResultOf(
            Result expectedResult, Func<Result, Result, bool> resultComparator)
            => ExpectExactlyNResultsOf(1, expectedResult, resultComparator);

        protected void ExpectExactlyNResultsOf(
            int expectedCount, Result expectedResult, Func<Result, Result, bool> resultComparator)
            => Assert.That(PairSelectResult(), Has.Exactly(expectedCount).Items.Matches<Result>(
                result => resultComparator(result, expectedResult)));

        [SetUp]
        public void SetUp()
        {
            _left = new List<Left>();
            _right = new List<Right>();
        }
    }
}

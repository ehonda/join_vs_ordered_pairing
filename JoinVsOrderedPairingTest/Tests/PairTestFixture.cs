using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JoinVsOrderedPairingTest.Tests
{
    //[TestFixtureSource(typeof(PairSelectImplementations))]
    public class PairTestFixture<Left, Right, Key, Result>
        where Key : IComparable<Key>
    {
        protected List<Left> _left;
        protected List<Right> _right;
        private readonly Func<Left, Key> _leftKeySelector;
        private readonly Func<Right, Key> _rightKeySelector;
        private readonly Func<Left, Right, Result> _resultSelector;
        private readonly Func<
            IEnumerable<Left>,
            IEnumerable<Right>,
            Func<Left, Key>,
            Func<Right, Key>,
            Func<Left, Right, Result>,
            IEnumerable<Result>> _pairSelect;

        public PairTestFixture(
            Func<Left, Key> leftKeySelector, 
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector,
            Func<
                IEnumerable<Left>,
                IEnumerable<Right>,
                Func<Left, Key>,
                Func<Right, Key>,
                Func<Left, Right, Result>,
                IEnumerable<Result>> pairSelect)
        {
            _leftKeySelector = leftKeySelector;
            _rightKeySelector = rightKeySelector;
            _resultSelector = resultSelector;
            _pairSelect = pairSelect;
        }

        protected IEnumerable<Result> PairSelectResult() =>
            _pairSelect(_left, _right, _leftKeySelector, _rightKeySelector, _resultSelector);


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

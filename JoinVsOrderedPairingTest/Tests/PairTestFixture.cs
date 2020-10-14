using NUnit.Framework;
using System;
using System.Collections.Generic;


// The type of our pair selector function. We would really like to do
//      using PairSelector<Left, Right, Key, Result> = ...
// but this is unfortunately not legal C# since type alias using directives
// can't have type parmeters, see 
//      https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces#using-directives
//
//  Func<
//      IEnumerable<Left>,
//      IEnumerable<Right>,
//      Func<Left, Key>,
//      Func<Right, Key>,
//      Func<Left, Right, Result>,
//      IEnumerable<Result>>

namespace JoinVsOrderedPairingTest.Tests
{
    public class PairTestFixture<Left, Right, Key, Result>
        where Key : IComparable<Key>
    {
        protected List<Left> _left;
        protected List<Right> _right;
        private readonly Func<Left, Key> _leftKeySelector;
        private readonly Func<Right, Key> _rightKeySelector;
        private readonly Func<Left, Right, Result> _resultSelector;
        private Func<
            IEnumerable<Left>,
            IEnumerable<Right>,
            Func<Left, Key>,
            Func<Right, Key>,
            Func<Left, Right, Result>,
            IEnumerable<Result>> _pairSelect;

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

        protected void TestWithPairSelect(
            Func<
                IEnumerable<Left>,
                IEnumerable<Right>,
                Func<Left, Key>,
                Func<Right, Key>,
                Func<Left, Right, Result>,
                IEnumerable<Result>> pairSelect,
            Action test)
        {
            _pairSelect = pairSelect;
            test();
        }

        [SetUp]
        public void SetUp()
        {
            _left = new List<Left>();
            _right = new List<Right>();
        }
    }
}

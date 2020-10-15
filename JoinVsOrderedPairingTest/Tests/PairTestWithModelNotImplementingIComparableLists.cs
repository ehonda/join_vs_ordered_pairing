using JoinVsOrderedPairingTest.Implementations.PairTestWithModelNotImplementingIComparableLists;
using JoinVsOrderedPairingTest.Models;
using NUnit.Framework;
using Key = System.Int32; // origin: ModelNotImplementingIComparable.cs

using PairSelector = System.Func<
                System.Collections.Generic.IEnumerable<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable>,
                System.Collections.Generic.IEnumerable<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, System.Int32>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, System.Int32>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, (JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable)>,
                System.Collections.Generic.IEnumerable<(JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable)>>;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class PairTestWithModelNotImplementingIComparableLists
        : PairTestFixtureWithIdenticalListTypesAndPairResult<ModelNotImplementingIComparable, Key>
    {
        public PairTestWithModelNotImplementingIComparableLists()
            : base(x => x.Key)
        {
        }

        private ModelNotImplementingIComparable SomeElement()
            => new ModelNotImplementingIComparable { Key = 1 };

        private ModelNotImplementingIComparable AnotherElement()
            => new ModelNotImplementingIComparable { Key = 2 };

        // This is a regression test: PairSelect did previously not use the keySelectors
        // to order the elements, which caused an error if the element's types did
        // not implement IComparable
        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        [TestCaseSource(typeof(PairSelectImplementations), "Join")]
        [TestCaseSource(typeof(PairSelectImplementations), "Naive")]
        [TestCaseSource(typeof(PairSelectImplementations), "ManuallyInlined")]
        public void Models_Not_Implementing_IComparable_Get_Sorted_And_Compared_Via_Key_Selector(PairSelector implementation)
            => TestWithImplementationAndWithSymmetricalSetups(implementation, (first, second) =>
            {
                first.AddRange(new[] { SomeElement(), AnotherElement() });
                second.AddRange(new[] { AnotherElement(), SomeElement() });

                ExpectNumberOfPairs(2);
                ExpectExactlyOnePairOf((SomeElement(), SomeElement()));
                ExpectExactlyOnePairOf((AnotherElement(), AnotherElement()));
            });
    }
}

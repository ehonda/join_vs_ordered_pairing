using JoinVsOrderedPairingTest.Models;
using NUnit.Framework;

using Key = System.Int32; // origin: ModelNotImplementingIComparable.cs

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
        [Test]
        public void Models_Not_Implementing_IComparable_Get_Sorted_And_Compared_Via_Key_Selector()
            => TestWithSymmetricalSetups((first, second) =>
            {
                first.AddRange(new[] { SomeElement(), AnotherElement() });
                second.AddRange(new[] { AnotherElement(), SomeElement() });

                ExpectNumberOfPairs(2);
                ExpectExactlyOnePairOf((SomeElement(), SomeElement()));
                ExpectExactlyOnePairOf((AnotherElement(), AnotherElement()));
            });
    }
}

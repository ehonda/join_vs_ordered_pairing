using JoinVsOrderedPairingTest.Implementations.PairTestWithStringLists;
using NUnit.Framework;
using System;
using System.Linq;

using PairSelector = System.Func<
                System.Collections.Generic.IEnumerable<string>,
                System.Collections.Generic.IEnumerable<string>,
                System.Func<string, string>,
                System.Func<string, string>,
                System.Func<string, string, (string, string)>,
                System.Collections.Generic.IEnumerable<(string, string)>>;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class PairTestWithStringLists
        : PairTestFixtureWithIdenticalListTypesAndPairResult<string, string>
    {
        public PairTestWithStringLists()
            : base(x => x)
        {
        }

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        public void Null_On_Right_Throws(PairSelector implementation)
            => TestWithPairSelect(implementation, () =>
                {
                    _right = null;
                    Assert.Throws<ArgumentNullException>(() => PairSelectResult().ToList());
                });

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        [TestCaseSource(typeof(PairSelectImplementations), "Join")]
        [TestCaseSource(typeof(PairSelectImplementations), "Naive")]
        public void If_One_List_Is_Empty_The_Result_Is_Empty(PairSelector implementation)
            => TestWithImplementationAndWithSymmetricalSetups(implementation, (first, second) =>
            {
                second.Add("A");
                ExpectNumberOfPairs(0);
            });

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        [TestCaseSource(typeof(PairSelectImplementations), "Join")]
        [TestCaseSource(typeof(PairSelectImplementations), "Naive")]
        public void One_Pair_At_The_Same_Position_Is_Returned(PairSelector implementation)
            => TestWithPairSelect(implementation, () =>
                {
                    _left.Add("A");
                    _right.Add("A");
                    ExpectNumberOfPairs(1);
                    ExpectExactlyOnePairOf(("A", "A"));
                });

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        [TestCaseSource(typeof(PairSelectImplementations), "Join")]
        [TestCaseSource(typeof(PairSelectImplementations), "Naive")]
        public void One_Pair_At_Different_Positions_Is_Returned(PairSelector implementation)
            => TestWithImplementationAndWithSymmetricalSetups(implementation, (first, second) =>
            {
                first.Add("B");
                second.AddRange(new[] { "A", "B" });
                ExpectNumberOfPairs(1);
                ExpectExactlyOnePairOf(("B", "B"));
            });

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        [TestCaseSource(typeof(PairSelectImplementations), "Naive")]
        public void Duplicate_Elements_Get_Paired_With_Other_Duplicates(PairSelector implementation)
            => TestWithPairSelect(implementation, () =>
                {
                    _left.AddRange(new[] { "A", "A" });
                    _right.AddRange(new[] { "A", "A" });
                    ExpectNumberOfPairs(2);
                    ExpectExactlyNPairsOf(2, ("A", "A"));
                });

        [TestCaseSource(typeof(PairSelectImplementations), "OrderBy")]
        public void Number_Of_Duplicate_Pairs_Is_The_Min_Of_Left_And_Right_Duplicates(PairSelector implementation)
            => TestWithImplementationAndWithSymmetricalSetups(implementation, (first, second) =>
            {
                first.AddRange(new[] { "A", "B" });
                second.AddRange(new[] { "A", "A" });
                ExpectNumberOfPairs(1);
                ExpectExactlyOnePairOf(("A", "A"));
            });
    }
}

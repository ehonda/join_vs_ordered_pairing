using JoinVsOrderedPairingTest.Implementations;
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
    [TestFixtureSource(typeof(PairSelectImplementations), "Implementations")]
    public class PairTestWithStringLists
        : PairTestFixtureWithIdenticalListTypesAndPairResult<string, string>
    {
        public PairTestWithStringLists(
            PairSelector pairSelect)
            : base(x => x, pairSelect)
        {
        }

        // TODO: The null tests should be in the base class / generic
        [Test]
        public void Null_On_Right_Throws()
        {
            _right = null;
            Assert.Throws<ArgumentNullException>(() => PairSelectResult().ToList());
        }

        [Test]
        public void If_One_List_Is_Empty_The_Result_Is_Empty()
            => TestWithSymmetricalSetups((first, second) =>
            {
                second.Add("A");
                ExpectNumberOfPairs(0);
            });

        [Test]
        public void One_Pair_At_The_Same_Position_Is_Returned()
        {
            _left.Add("A");
            _right.Add("A");
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf(("A", "A"));
        }

        [Test]
        public void One_Pair_At_Different_Positions_Is_Returned()
            => TestWithSymmetricalSetups((first, second) =>
            {
                first.Add("B");
                second.AddRange(new[] { "A", "B" });
                ExpectNumberOfPairs(1);
                ExpectExactlyOnePairOf(("B", "B"));
            });

        [Test]
        public void Duplicate_Elements_Get_Paired_With_Other_Duplicates()
        {
            _left.AddRange(new[] { "A", "A" });
            _right.AddRange(new[] { "A", "A" });
            ExpectNumberOfPairs(2);
            ExpectExactlyNPairsOf(2, ("A", "A"));
        }

        [Test]
        public void Number_Of_Duplicate_Pairs_Is_The_Min_Of_Left_And_Right_Duplicates()
            => TestWithSymmetricalSetups((first, second) =>
            {
                first.AddRange(new[] { "A", "B" });
                second.AddRange(new[] { "A", "A" });
                ExpectNumberOfPairs(1);
                ExpectExactlyOnePairOf(("A", "A"));
            });
    }
}

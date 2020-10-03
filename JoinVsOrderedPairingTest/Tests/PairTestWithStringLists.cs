using NUnit.Framework;

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

        // TODO: null tests

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

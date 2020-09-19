using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JoinVsOrderedPairingTest
{
    public class PairTest
    {
        private List<string> _left;
        private List<string> _right;

        private IEnumerable<(string, string)> Pairing() => _left.Pair(_right);

        private void ExpectNumberOfPairs(int expectedCount)
            => Assert.That(Pairing(), Has.Exactly(expectedCount).Items);

        private void ExpectExactlyOnePairOf((string, string) expectedPair)
            => ExpectExactlyNPairsOf(1, expectedPair);

        private void ExpectExactlyNPairsOf(int expectedCount, (string, string) expectedPair)
            => Assert.That(Pairing(), Has.Exactly(expectedCount).Items.Matches<(string, string)>(
                pair => pair == expectedPair));

        private void TestWithSymmetricalSetups(Action<List<string>, List<string>> test)
        {
            Setup();
            test(_left, _right);
            Setup();
            test(_right, _left);
        }

        [SetUp]
        public void Setup()
        {
            _left = new List<string>();
            _right = new List<string>();
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

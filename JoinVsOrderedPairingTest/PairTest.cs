using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using Left = System.Int32;
using Right = System.Int32;

namespace JoinVsOrderedPairingTest
{
    public class PairTest
    {
        private List<Left> _left;
        private List<Right> _right;

        private IEnumerable<(Left, Right)> Pairing() => _left.Pair(_right);

        private void ExpectNumberOfPairs(int expectedCount)
            => Assert.That(Pairing(), Has.Exactly(expectedCount).Items);

        private void ExpectExactlyOnePairOf((Left, Right) expectedPair)
            => ExpectExactlyNPairsOf(1, expectedPair);

        private void ExpectExactlyNPairsOf(int expectedCount, (Left, Right) expectedPair)
            => Assert.That(Pairing(), Has.Exactly(expectedCount).Items.Matches<(Left, Right)>(
                pair => pair == expectedPair));

        [SetUp]
        public void Setup()
        {
            _left = new List<Left>();
            _right = new List<Right>();
        }

        // TODO: null tests

        [Test]
        public void If_One_List_Is_Empty_The_Result_Is_Empty()
        {
            _left.Clear();
            _right.Add(1);
            ExpectNumberOfPairs(0);

            Setup();
            _left.Add(1);
            _right.Clear();
            ExpectNumberOfPairs(0);
        }

        [Test]
        public void One_Pair_At_The_Same_Position_Is_Returned()
        {
            _left.Add(1);
            _right.Add(1);
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf((1, 1));
        }

        [Test]
        public void One_Pair_At_Different_Positions_Is_Returned()
        {
            _left.Add(2);
            _right.AddRange(new[] { 1, 2 });
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf((2, 2));

            Setup();
            _right.Add(2);
            _left.AddRange(new[] { 1, 2 });
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf((2, 2));
        }

        [Test]
        public void Duplicate_Elements_Get_Paired_With_Other_Duplicates()
        {
            _left.AddRange(new[] { 1, 1 });
            _right.AddRange(new[] { 1, 1 });
            ExpectNumberOfPairs(2);
            ExpectExactlyNPairsOf(2, (1, 1));
        }

        [Test]
        public void Number_Of_Duplicate_Pairs_Is_The_Min_Of_Left_And_Right_Duplicates()
        {
            _left.AddRange(new[] { 1, 2 });
            _right.AddRange(new[] { 1, 1 });
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf((1, 1));

            Setup();
            _right.AddRange(new[] { 1, 2 });
            _left.AddRange(new[] { 1, 1 });
            ExpectNumberOfPairs(1);
            ExpectExactlyOnePairOf((1, 1));
        }
    }
}
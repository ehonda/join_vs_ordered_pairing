using JoinVsOrderedPairing.Extensions;
using JoinVsOrderedPairingTest.TestData;
using NUnit.Framework;
using System;
using System.Linq;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class ShuffleTest
    {
        private void ExpectSuccessAtLeastOnce<Actual>(
            int tries, Func<Actual> generateActual, Func<Actual, bool> predicate)
            => Assert.True(
                Enumerable
                .Repeat(generateActual(), tries)
                .Select(actual => predicate(actual))
                .Aggregate((x, y) => x || y));

        [Test]
        public void Shuffled_List_Contains_The_Same_Elements()
        {
            var elements = Enumerable.Range(0, 10).ToArray();
            elements.Shuffle();

            foreach (var element in Enumerable.Range(0, 10))
                Assert.That(elements, Has.Exactly(1).Items.EqualTo(element));
        }

        // This test may fail with probability 1 / 1000! ~ 2.5 * 10 ^ (-2568),
        // since there are 1000! permutations of elements and only one (the
        // identical permutation) fails this test.
        [Test]
        public void At_Least_One_Element_Got_Shuffled()
        {
            var elements = Enumerable.Range(0, 1000).ToArray();
            elements.Shuffle();

            // If an element x_i got shuffled to a different position,
            // it will hold that x_i != i, since x_i starts out at
            // position i.
            Assert.That(elements.Zip(Enumerable.Range(0, 1000)), 
                Has.Some.Matches<(int, int)>(
                    x_i => x_i.Item1 != x_i.Item2));
        }

        // This test fails for any given valid position with probablity
        //      1 / (2 ^ 10) ^ 10 ~ 8 * 10 ^ (-31) =: p
        // It then fails for n valid runs with probability
        //      np ~ 8n * 10 ^ (-31)
        //
        // The number of runs is denoted next to the different test case
        // sources, since p is so small compared to the reciprocal of
        // these n, the failure probabilities for all test case sources 
        // will roughly be of the same order of magnitude:
        //      ~ 10 ^ (-31)
        [TestCaseSource(typeof(ShuffleTestIndices), "Boundaries")]
        [TestCaseSource(typeof(ShuffleTestIndices), "SixteenEquidistant")]
        public void Element_At_Position_Gets_Shuffled(int position)
        {
            var n = ShuffleTestIndices.Length;
            if (position < 0 || position >= n)
                Assert.Fail("Out of bounds position can not be tested.");

            ExpectSuccessAtLeastOnce(
                10,
                () => Enumerable.Range(0, n).ToArray().Shuffle(),
                actual => actual[position] != position);
        }
    }
}

using JoinVsOrderedPairing.Extensions;
using JoinVsOrderedPairingTest.TestData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class ShuffleTest
    {
        private void ExpectSuccessAtLeastOnce(
            int tries, Func<bool> predicate, string message)
            => Assert.True(
                Enumerable
                    .Repeat(0, tries)              // Can't do .Repeat(predicate(), tries)
                    .Select(_ => predicate())      // since that will only invoke generate once
                    .Aggregate((x, y) => x || y),
                message);

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

        // TODO: Explain how to get these numbers
        //
        // The probability of success when doing n runs per index
        // for m indices, with a list of length l is
        //      (1 - 1 / l ^ n) ^ m
        [TestCaseSource(typeof(ShuffleTestIndices), "All")]
        public void Element_At_Position_Gets_Shuffled((int, string) positionAndMessage)
        {
            var n = ShuffleTestIndices.Length;
            var position = positionAndMessage.Item1;
            var failureMessage = positionAndMessage.Item2;

            if (position < 0 || position >= n)
                Assert.Fail("Out of bounds position can not be tested.");

            IList<int> ShuffledElements()
                => Enumerable.Range(0, n).ToArray().Shuffle();

            static bool NoFixpointAt(IList<int> x, int i) => x[i] != i;

            ExpectSuccessAtLeastOnce(
                ShuffleTestIndices.Tries,
                () => NoFixpointAt(ShuffledElements(), position),
                failureMessage);
        }
    }
}

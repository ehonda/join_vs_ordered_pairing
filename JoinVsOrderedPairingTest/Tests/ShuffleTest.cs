using JoinVsOrderedPairing.Extensions;
using NUnit.Framework;
using System.Linq;

namespace JoinVsOrderedPairingTest.Tests
{
    [TestFixture]
    public class ShuffleTest
    {
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
            Assert.That(elements.Zip(Enumerable.Range(0, 1000)), Has.Some.Matches<(int, int)>(
                x_i => x_i.Item1 != x_i.Item2));
        }

        // This test fails for all arrangements that have 0 as their first
        // element, which happens with probability 1 / 100000
        [Test]
        public void First_Element_Gets_Shuffled()
        {
            var elements = Enumerable.Range(0, 100000).ToArray();
            elements.Shuffle();

            Assert.That(elements[0] != 0);
        }

        // This test fails for all arrangements that have 0 as their first
        // element, which happens with probability 1 / 100000
        [Test]
        public void Last_Element_Gets_Shuffled()
        {
            var elements = Enumerable.Range(0, 100000).ToArray();
            elements.Shuffle();

            Assert.That(elements[99999] != 99999);
        }
    }
}

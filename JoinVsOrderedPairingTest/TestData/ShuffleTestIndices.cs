using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairingTest.TestData
{
    public static class ShuffleTestIndices
    {
        public const int Length = 1 << 10;

        public static IEnumerable<int> Boundaries
        {
            get
            {
                yield return 0;
                yield return Length - 1;
            }
        }

        public static IEnumerable<int> SixteenEquidistant
            => Enumerable.Range(0, 16).Select(x => x * Length / 16);
    }
}

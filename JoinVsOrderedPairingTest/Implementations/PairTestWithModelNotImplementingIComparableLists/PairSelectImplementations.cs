using JoinVsOrderedPairing.Extensions;
using System.Collections.Generic;

using PairSelector = System.Func<
                System.Collections.Generic.IEnumerable<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable>,
                System.Collections.Generic.IEnumerable<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, System.Int32>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, System.Int32>,
                System.Func<JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, (JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable)>,
                System.Collections.Generic.IEnumerable<(JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable, JoinVsOrderedPairingTest.Models.ModelNotImplementingIComparable)>>;


namespace JoinVsOrderedPairingTest.Implementations.PairTestWithModelNotImplementingIComparableLists
{
    public class PairSelectImplementations
    {
        public static IEnumerable<PairSelector> OrderBy
        {
            get
            {
                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.PairSelect(right, leftKeySelector, rightKeySelector, resultSelector));
            }
        }

        public static IEnumerable<PairSelector> Join
        {
            get
            {
                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.JoinSelect(right, leftKeySelector, rightKeySelector, resultSelector));
            }
        }

        public static IEnumerable<PairSelector> Naive
        {
            get
            {
                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.PairSelectNaive(right, leftKeySelector, rightKeySelector, resultSelector));
            }
        }
    }
}

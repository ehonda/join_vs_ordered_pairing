using JoinVsOrderedPairing.Extensions;
using System.Collections.Generic;

using PairSelector = System.Func<
                System.Collections.Generic.IEnumerable<string>,
                System.Collections.Generic.IEnumerable<string>,
                System.Func<string, string>,
                System.Func<string, string>,
                System.Func<string, string, (string, string)>,
                System.Collections.Generic.IEnumerable<(string, string)>>;

namespace JoinVsOrderedPairingTest.Implementations
{
    public class PairSelectImplementations
    {
        public static IEnumerable<PairSelector> Implementations
        {
            get
            {
                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.PairSelect(right, leftKeySelector, rightKeySelector, resultSelector));

                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.JoinSelect(right, leftKeySelector, rightKeySelector, resultSelector));

                yield return new PairSelector(
                    (left, right, leftKeySelector, rightKeySelector, resultSelector)
                        => left.PairSelectNaive(right, leftKeySelector, rightKeySelector, resultSelector));
            }
        }
    }
}

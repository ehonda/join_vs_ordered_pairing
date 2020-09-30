using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairing.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Pairs elements in <paramref name="left"/> with matching elements
        /// in <paramref name="right"/>, by comparison on keys extract with
        /// <paramref name="leftKeyExtractor"/> and <paramref name="rightKeyExtractor"/>.
        /// Inputs must be sorted on these keys.
        /// </summary>
        /// <typeparam name="Left">Type of left elements</typeparam>
        /// <typeparam name="Right">Type of right elements</typeparam>
        /// <typeparam name="Key">
        ///     Type of keys to compare, must implement <see cref="IComparable{Key}"/>
        /// </typeparam>
        /// <param name="left">Left elements</param>
        /// <param name="right">Right elements</param>
        /// <param name="leftKeyExtractor">Left key extractor function</param>
        /// <param name="rightKeyExtractor">Right key extractor function</param>
        /// <returns>The paired elements</returns>
        public static IEnumerable<Result> PairSelectOnOrderedInputs<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeyExtractor,
            Func<Right, Key> rightKeyExtractor,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
        {
            #region DSL Setup

            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();

            Left LeftElement() => leftEnumerator.Current;
            Right RightElement() => rightEnumerator.Current;

            Key LeftKey() => leftKeyExtractor(LeftElement());
            Key RightKey() => rightKeyExtractor(RightElement());

            var continuePairing = true;

            bool LeftEnumeratorMightCatchUp() => continuePairing && LeftKey().IsLowerThan(RightKey());
            bool RightEnumeratorMightCatchUp() => continuePairing && RightKey().IsLowerThan(LeftKey());

            void AdvanceLeft() => continuePairing &= leftEnumerator.MoveNext();
            void AdvanceRight() => continuePairing &= rightEnumerator.MoveNext();
            void AdvanceBoth() { AdvanceLeft(); AdvanceRight(); }

            bool PairingFound() => continuePairing && LeftKey().IsEqualTo(RightKey());

            #endregion

            #region Algorithm

            AdvanceBoth();
            var paired = new List<Result>();
            while (continuePairing)
            {
                if (PairingFound())
                {
                    paired.Add(resultSelector(LeftElement(), RightElement()));
                    AdvanceBoth();
                }

                while (LeftEnumeratorMightCatchUp())
                    AdvanceLeft();

                while (RightEnumeratorMightCatchUp())
                    AdvanceRight();
            }
            return paired;

            #endregion
        }

        public static IEnumerable<Result> PairSelect<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeyExtractor,
            Func<Right, Key> rightKeyExtractor,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
            => left.OrderBy(x => x).PairSelectOnOrderedInputs(right.OrderBy(x => x),
                leftKeyExtractor, rightKeyExtractor, resultSelector);

        public static IEnumerable<(T, T)> Pair<T>(this IEnumerable<T> left, IEnumerable<T> right)
            where T : IComparable<T>
            => left.PairSelectOnOrderedInputs(right, x => x, x => x, (left, right) => (left, right));

        public static IEnumerable<Result> JoinSelect<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeyExtractor,
            Func<Right, Key> rightKeyExtractor,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
            => left.Join(right, leftKeyExtractor, rightKeyExtractor, resultSelector);

        public static IEnumerable<(T, T)> JoinPair<T>(this IEnumerable<T> left, IEnumerable<T> right)
            where T : IComparable<T>
            => left.JoinSelect(right, x => x, x => x, (left, right) => (left, right));
    }
}

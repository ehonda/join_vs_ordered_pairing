using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinVsOrderedPairing.Extensions
{
    public static class IEnumerableExtensions
    {
        #region PairSelect implementations

        #region Pair select on ordered inputs

        /// <summary>
        /// Pairs and projects elements in <paramref name="left"/> with matching elements
        /// in <paramref name="right"/>, by comparison on keys extracted with
        /// <paramref name="leftKeySelector"/> and <paramref name="rightKeySelector"/>.
        /// Inputs must be sorted on these keys. The result elements are retrieved by
        /// projecting the paired up elements via <paramref name="resultSelector"/>.
        /// </summary>
        /// <typeparam name="Left">Type of left elements.</typeparam>
        /// <typeparam name="Right">Type of right elements.</typeparam>
        /// <typeparam name="Key">
        ///     Type of keys to compare, must implement <see cref="IComparable{Key}"/>.
        /// </typeparam>
        /// <typeparam name="Result">Type of result elements.</typeparam>
        /// <param name="left"><typeparamref name="Left"/> elements.</param>
        /// <param name="right"><typeparamref name="Right"/> elements.</param>
        /// <param name="leftKeySelector">
        ///     Function to project elements of type <typeparamref name="Left"/>
        ///     onto their corresponding <typeparamref name="Key"/>.
        /// </param>
        /// <param name="rightKeySelector">
        ///     Function to project elements of type <typeparamref name="Right"/>
        ///     onto their corresponding <typeparamref name="Key"/>.
        /// </param>
        /// <param name="resultSelector">
        ///     Function to project paired elements of type
        ///     (<typeparamref name="Left"/>, <typeparamref name="Right"/>)
        ///     onto a result element of type <typeparamref name="Result"/>.
        /// </param>
        /// <returns>
        ///     The <typeparamref name="Result"/> elements retrieved by
        ///     projecting paired up elements from the input sequences.
        /// </returns>
        public static IEnumerable<Result> PairSelectOnOrderedInputs<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeySelector,
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
        {
            Guard.Against.Null(left, "left");
            Guard.Against.Null(right, "right");
            Guard.Against.Null(leftKeySelector, "leftKeySelector");
            Guard.Against.Null(rightKeySelector, "rightKeySelector");
            Guard.Against.Null(resultSelector, "resultSelector");

            #region DSL Setup

            var leftEnumerator = left.GetEnumerator();
            var rightEnumerator = right.GetEnumerator();

            Left LeftElement() => leftEnumerator.Current;
            Right RightElement() => rightEnumerator.Current;

            Key LeftKey() => leftKeySelector(LeftElement());
            Key RightKey() => rightKeySelector(RightElement());

            var continuePairing = true;

            bool LeftEnumeratorMightCatchUp() => continuePairing && LeftKey().IsLowerThan(RightKey());
            bool RightEnumeratorMightCatchUp() => continuePairing && RightKey().IsLowerThan(LeftKey());

            void AdvanceLeft() => continuePairing &= leftEnumerator.MoveNext();
            void AdvanceRight() => continuePairing &= rightEnumerator.MoveNext();
            void AdvanceBoth() { AdvanceLeft(); AdvanceRight(); }

            bool PairingFound() => continuePairing && LeftKey().IsEqualTo(RightKey());

            #endregion

            #region Algorithm

            return Pairs();

            IEnumerable<Result> Pairs()
            {
                AdvanceBoth();
                while (continuePairing)
                {
                    if (PairingFound())
                    {
                        yield return resultSelector(LeftElement(), RightElement());
                        AdvanceBoth();
                    }

                    while (LeftEnumeratorMightCatchUp())
                        AdvanceLeft();

                    while (RightEnumeratorMightCatchUp())
                        AdvanceRight();
                }
                yield break;
            }

            #endregion
        }

        public static IEnumerable<Result> PairSelect<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeySelector,
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
            => left.OrderBy(x => leftKeySelector(x)).PairSelectOnOrderedInputs(
                right.OrderBy(x => rightKeySelector(x)),
                leftKeySelector, rightKeySelector, resultSelector);

        public static IEnumerable<(T, T)> Pair<T>(this IEnumerable<T> left, IEnumerable<T> right)
            where T : IComparable<T>
            => left.PairSelectOnOrderedInputs(right, x => x, x => x, (left, right) => (left, right));

        #endregion

        #region Pair select via join

        public static IEnumerable<Result> JoinSelect<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeySelector,
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
            => left.Join(right, leftKeySelector, rightKeySelector, resultSelector);

        public static IEnumerable<(T, T)> JoinPair<T>(this IEnumerable<T> left, IEnumerable<T> right)
            where T : IComparable<T>
            => left.JoinSelect(right, x => x, x => x, (left, right) => (left, right));

        #endregion

        #region Pair select naive implementation

        public static IEnumerable<Result> PairSelectNaive<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeySelector,
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
        {
            foreach (var leftElement in left)
                foreach (var rightElement in right)
                    if (leftKeySelector(leftElement).IsEqualTo(rightKeySelector(rightElement)))
                    {
                        yield return resultSelector(leftElement, rightElement);
                        break;
                    }
            yield break;
        }

        #endregion

        #region Manually inlined

        public static IEnumerable<Result> PairSelectManuallyInlinedOnOrderedInputs
            <Left, Right, Key, Result>(
                this IEnumerable<Left> left, IEnumerable<Right> right,
                Func<Left, Key> leftKeySelector,
                Func<Right, Key> rightKeySelector,
                Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
        {
            #region Algorithm

            return Pairs();

            IEnumerable<Result> Pairs()
            {
                var continuePairing = true;

                var leftEnumerator = left.GetEnumerator();
                var rightEnumerator = right.GetEnumerator();

                continuePairing &= leftEnumerator.MoveNext();
                continuePairing &= rightEnumerator.MoveNext();
                while (continuePairing)
                {
                    if (continuePairing && leftKeySelector(leftEnumerator.Current)
                        .CompareTo(rightKeySelector(rightEnumerator.Current)) == 0)
                    {
                        yield return resultSelector(leftEnumerator.Current, rightEnumerator.Current);
                        continuePairing &= leftEnumerator.MoveNext();
                        continuePairing &= rightEnumerator.MoveNext();
                    }

                    while (continuePairing && leftKeySelector(leftEnumerator.Current)
                        .CompareTo(rightKeySelector(rightEnumerator.Current)) == -1)
                        continuePairing &= leftEnumerator.MoveNext();

                    while (continuePairing && rightKeySelector(rightEnumerator.Current)
                        .CompareTo(leftKeySelector(leftEnumerator.Current)) == -1)
                        continuePairing &= rightEnumerator.MoveNext();
                }
                yield break;
            }

            #endregion
        }

        public static IEnumerable<Result> PairSelectManuallyInlined<Left, Right, Key, Result>(
            this IEnumerable<Left> left, IEnumerable<Right> right,
            Func<Left, Key> leftKeySelector,
            Func<Right, Key> rightKeySelector,
            Func<Left, Right, Result> resultSelector)
            where Key : IComparable<Key>
            => left.OrderBy(leftKeySelector).PairSelectManuallyInlinedOnOrderedInputs(
                right.OrderBy(rightKeySelector), leftKeySelector, rightKeySelector, resultSelector);

        #endregion

        #endregion
    }
}

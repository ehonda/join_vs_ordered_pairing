using System;

namespace JoinVsOrderedPairing.Extensions
{
    public static class IComparableExtensions
    {
        public static bool IsGreaterThan<Key>(this Key left, Key right)
            where Key : IComparable<Key>
            => left.CompareTo(right) == 1;

        public static bool IsLowerThan<Key>(this Key left, Key right)
            where Key : IComparable<Key>
            => left.CompareTo(right) == -1;

        public static bool IsGreaterOrEqualThan<Key>(this Key left, Key right)
            where Key : IComparable<Key>
            => left.CompareTo(right) >= 0;

        public static bool IsLowerOrEqualThan<Key>(this Key left, Key right)
            where Key : IComparable<Key>
            => left.CompareTo(right) <= 0;

        public static bool IsEqualTo<Key>(this Key left, Key right)
            where Key : IComparable<Key>
            => left.CompareTo(right) == 0;
    }
}

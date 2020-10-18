using System;
using System.Collections.Generic;

namespace JoinVsOrderedPairing.Extensions
{
    public static class IListExtensions
    {
        /// <summary>
        /// This is an implementation of the Fisher-Yates shuffle. For more
        /// information please consult the following links
        /// <list type="bullet">
        /// <item>
        ///     https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
        /// </item>
        /// <item>https://stackoverflow.com/q/273313</item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="elements">Elements to shuffle.</param>
        public static void Shuffle<T>(this IList<T> elements)
        {
            var random = new Random();
            var n = elements.Count;

            for (int i = n - 1; i > 0; --i)
            {
                var j = random.Next(i + 1);
                var old_element_i = elements[i];
                elements[i] = elements[j];
                elements[j] = old_element_i;
            }
        }
    }
}

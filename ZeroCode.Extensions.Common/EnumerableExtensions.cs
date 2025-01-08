using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extensions methods for classes that implements <see cref="IEnumerable" />
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Safely concatenate two sequences, that means <paramref name="source" /> or <paramref name="other" /> can be
        ///     <see langword="null" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IEnumerable<T> SafeConcat<T>(
            this IEnumerable<T>? source,
            IEnumerable<T>? other)
        {
            return (source ?? Enumerable.Empty<T>()).Concat(other ?? Enumerable.Empty<T>());
        }

        /// <summary>
        ///     Returns a hash code computed for sequence. It's useful for check equality of two sequences.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        ///     Solution got from <seealso href="https://stackoverflow.com/a/68680635">StackOverflow</seealso>
        /// </remarks>
        public static int GetCollectionHashCode<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return ((IStructuralEquatable)(source as T[] ?? source.ToArray())).GetHashCode(EqualityComparer<T>.Default);
        }

        /// <summary>
        ///     Merging two sequences for concatenate sequences and merge duplicates via <paramref name="mergingFunction" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <param name="equalityFunction">Function that returns value used for identify equality of element</param>
        /// <param name="mergingFunction">Function that merge duplicates in concatenated sequence</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Merge<T, TValue>(
            this IEnumerable<T> source,
            IEnumerable<T> other,
            Func<T, TValue> equalityFunction,
            Func<T, T, T> mergingFunction)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (equalityFunction == null) throw new ArgumentNullException(nameof(equalityFunction));
            if (mergingFunction == null) throw new ArgumentNullException(nameof(mergingFunction));

            return source.Concat(other)
                .GroupBy(equalityFunction)
                .Select(grouping => grouping.Aggregate(mergingFunction));
        }
    }
}
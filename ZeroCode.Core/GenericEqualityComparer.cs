using System;
using System.Collections.Generic;
using System.Linq;

namespace ZeroCode
{
    /// <summary>
    ///     Generic implementation of <see cref="IEqualityComparer{T}" /> that can be used for transforming equality comparer
    ///     function into implementation of <see cref="IEqualityComparer{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        ///     Equality comparer function
        /// </summary>
        private readonly Func<T, T, bool> _comparer;

        /// <summary>
        ///     Functions that returns all candidates for calculating object hash code. Its important because LINQ uses hash codes
        ///     often for equality compare.
        /// </summary>
        private readonly Func<T, object?[]> _hashCodeCandidatesFactory;

        /// <param name="comparer">
        ///     <inheritdoc cref="_comparer" />
        /// </param>
        /// <param name="hashCodeCandidatesFactory">
        ///     <inheritdoc cref="_hashCodeCandidatesFactory" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericEqualityComparer(Func<T, T, bool> comparer, Func<T, object?[]> hashCodeCandidatesFactory)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _hashCodeCandidatesFactory = hashCodeCandidatesFactory ??
                                         throw new ArgumentNullException(nameof(hashCodeCandidatesFactory));
        }

        /// <inheritdoc />
        public bool Equals(T x, T y)
        {
            return (x, y) switch
            {
                (null, null) => true,
                (null, _) => false,
                (_, null) => false,
                (_, _) when ReferenceEquals(x, y) => true,
                _ => _comparer(x, y)
            };
        }

        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            unchecked
            {
                return _hashCodeCandidatesFactory(obj)
                    .Aggregate(0, (current, candidate) =>
                        (current * 397) ^ (candidate != null ? candidate.GetHashCode() : 0)
                    );
            }
        }
    }
}
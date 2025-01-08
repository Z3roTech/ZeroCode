using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extension methods for any objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Executing operator <see langword="as" /> as extension method for use in method chaining.
        /// </summary>
        /// <remarks>
        ///     Example: <br />
        ///     <example>
        ///         <c>(fooBar <see langword="as" /> Bar)?.SomeProperty</c>
        ///     </example>
        ///     <br />
        ///     <example>
        ///         <c>fooBar.<see langword="As" />&lt;Bar>?.SomeProperty</c>
        ///     </example>
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        [return: MaybeNull]
        public static T As<T>(this object? @this) where T : class?
        {
            return @this as T;
        }

        /// <summary>
        ///     Cast <paramref name="this" /> to type <typeparamref name="T" /> as extension method for use in method
        ///     chaining.<br />
        ///     <remarks>
        ///         <b>Warning!</b> If class define <see langword="explicit" /> operator, this method will not work for
        ///         explicit cast, use default cast expression instead!
        ///     </remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T Cast<T>(this object @this)
        {
            return (T)@this;
        }

        /// <summary>
        ///     Cast <paramref name="this" /> to type <typeparamref name="T" /> with precheck value to be
        ///     <see cref="DBNull.Value" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns>
        ///     If value is equal <see cref="DBNull.Value" /> then <see langword="default" /> of type <typeparamref name="T" />,
        ///     else <paramref name="this" /> casted to type <typeparamref name="T" />
        /// </returns>
        [return: MaybeNull]
        public static T DbCast<T>(this object @this)
        {
            if (@this == DBNull.Value) return default;

            return (T)@this;
        }

        /// <summary>
        ///     Check value of <paramref name="source" /> is equal to <see langword="default" /> of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDefault<T>([AllowNull] this T source)
        {
            // null allowed here
            return EqualityComparer<T>.Default.Equals(source!, default!);
        }

        /// <summary>
        ///     Set to <paramref name="target" /> value from <paramref name="value" /> if <paramref name="target" /> have
        ///     <see langword="default" /> value of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetIfDefault<T>([AllowNull] ref T target, [AllowNull] T value)
        {
            if (target.IsDefault()) target = value!;
        }
    }
}
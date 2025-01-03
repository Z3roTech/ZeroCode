using System;
using System.Linq;
using System.Reflection;

namespace ZeroCode.Extensions.Common
{
    /// <summary>
    ///     Extension methods for working with <see cref="Attribute" />
    /// </summary>
    public static class AttributesExtensions
    {
        /// <summary>
        ///     Get first attribute of type <typeparamref name="TAttribute" /> from type <paramref name="type" />
        /// </summary>
        /// <typeparam name="TAttribute">Generic type of <see cref="Attribute" /> child</typeparam>
        /// <param name="type">Type of class, struct or interface attribute gets from</param>
        /// <returns></returns>
        public static TAttribute? GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttribute(typeof(TAttribute)).As<TAttribute>();
        }

        /// <summary>
        ///     Get array of attributes of type <typeparamref name="TAttribute" /> from type <paramref name="type" />
        /// </summary>
        /// <typeparam name="TAttribute">Generic type of <see cref="Attribute" /> child</typeparam>
        /// <param name="type">Type of class, struct or interface attribute gets from</param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute))
                .Select(attribute => attribute.As<TAttribute>())
                .Where(attribute => attribute != null)
                .ToArray()!;
        }

        /// <summary>
        ///     Get first attribute of type <typeparamref name="TAttribute" /> from member of type <paramref name="type" />
        /// </summary>
        /// <typeparam name="TAttribute">Generic type of <see cref="Attribute" /> child</typeparam>
        /// <param name="type">Type of class, struct or interface attribute gets from</param>
        /// <param name="memberName">Name of member of type <paramref name="type" /></param>
        /// <returns></returns>
        public static TAttribute? GetAttribute<TAttribute>(this Type type, string memberName)
            where TAttribute : Attribute
        {
            return type.GetMember(memberName)
                .FirstOrDefault()
                ?.GetCustomAttribute(typeof(TAttribute))
                .As<TAttribute>();
        }

        /// <summary>
        ///     Get array of attributes of type <typeparamref name="TAttribute" /> from member of type <paramref name="type" />
        /// </summary>
        /// <typeparam name="TAttribute">Generic type of <see cref="Attribute" /> child</typeparam>
        /// <param name="type">Type of class, struct or interface attribute gets from</param>
        /// <param name="memberName">Name of member of type <paramref name="type" /></param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(this Type type, string memberName)
            where TAttribute : Attribute
        {
            return (type.GetMember(memberName)
                        .FirstOrDefault()
                        ?.GetCustomAttributes(typeof(TAttribute))
                        .Select(attribute => attribute.As<TAttribute>())
                        .Where(attribute => attribute != null)
                        .ToArray()
                    ?? Array.Empty<TAttribute>())!;
        }
    }
}
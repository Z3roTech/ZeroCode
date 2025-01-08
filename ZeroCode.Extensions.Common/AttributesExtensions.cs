using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extension methods for working with <see cref="Attribute" />
    /// </summary>
    public static class AttributesExtensions
    {
        /// <summary>
        ///     Returns first attribute of type <typeparamref name="TAttribute" /> from type <paramref name="type" />
        /// </summary>
        /// <typeparam name="TAttribute">Generic type of <see cref="Attribute" /> child</typeparam>
        /// <param name="type">Type of class, struct or interface attribute gets from</param>
        /// <returns></returns>
        public static TAttribute? GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttribute(typeof(TAttribute)).As<TAttribute>();
        }

        /// <summary>
        ///     Returns array of attributes of type <typeparamref name="TAttribute" /> from type <paramref name="type" />
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
        ///     Returns first attribute of type <typeparamref name="TAttribute" /> from member of type <paramref name="type" />
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
        ///     Returns array of attributes of type <typeparamref name="TAttribute" /> from member of type <paramref name="type" />
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

        /// <summary>
        ///     Returns value of <see cref="DescriptionAttribute" /> of enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Value of enum <typeparamref name="T" /></param>
        /// <returns></returns>
        public static string GetDescription<T>(this T value) where T : Enum
        {
            var enumValue = value.ToString();
            return typeof(T).GetAttribute<DescriptionAttribute>(enumValue)?.Description ?? enumValue;
        }
    }
}
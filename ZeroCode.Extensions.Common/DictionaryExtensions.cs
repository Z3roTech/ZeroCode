using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extensions methods for Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        ///     Add a new pair of key and value to dictionary if dictionary not contains key, else update value of the specified
        ///     key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value">Value that will be used both for add new value or update existing value in dictionary</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            [MaybeNull] TValue value)
            where TKey : notnull
        {
            return source.AddOrUpdate(key, value, value);
        }

        /// <summary>
        ///     <inheritdoc cref="AddOrUpdate{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="addValue">Value that will be added if dictionary has not contain key</param>
        /// <param name="updateValue">New value of key if dictionary contain it</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            [MaybeNull] TValue addValue,
            [MaybeNull] TValue updateValue)
            where TKey : notnull
        {
            if (!source.TryAdd(key, addValue)) return source[key] = updateValue;

            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="AddOrUpdate{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="addValue">Value that will be added if dictionary has not contain key</param>
        /// <param name="updateValueFactory">Function that be used for getting value on update</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            [MaybeNull] TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
            where TKey : notnull
        {
            if (!source.TryAdd(key, addValue)) return source[key] = updateValueFactory(key, source[key]);

            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="AddOrUpdate{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="addValueFactory">Function that be used for getting that be added to dictionary</param>
        /// <param name="updateValueFactory">Function that be used for getting value on update</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
            where TKey : notnull
        {
            if (source.ContainsKey(key)) return source[key] = updateValueFactory(key, source[key]);

            source.Add(key, addValueFactory(key));
            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="AddOrUpdate{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="addValue">Value that will be added if dictionary has not contain key</param>
        /// <param name="updateValueFactory">Function that be used for getting value on update</param>
        /// <param name="updateFactoryArgument">
        ///     Additional data that will be provided to <paramref name="updateValueFactory" />
        /// </param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue, TArg>(this IDictionary<TKey, TValue> source, TKey key,
            [MaybeNull] TValue addValue, Func<TKey, TValue, TArg, TValue> updateValueFactory,
            TArg updateFactoryArgument)
            where TKey : notnull
        {
            if (!source.TryAdd(key, addValue))
                return source[key] = updateValueFactory(key, source[key], updateFactoryArgument);

            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="AddOrUpdate{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="addValueFactory">Function that be used for getting that be added to dictionary</param>
        /// <param name="updateValueFactory">Function that be used for getting value on update</param>
        /// <param name="updateFactoryArgument">
        ///     Additional data that will be provided to <paramref name="updateValueFactory" />
        /// </param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue, TArg>(this IDictionary<TKey, TValue> source, TKey key,
            Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory,
            TArg updateFactoryArgument)
            where TKey : notnull
        {
            if (source.ContainsKey(key))
                return source[key] = updateValueFactory(key, source[key], updateFactoryArgument);

            source.Add(key, addValueFactory(key));
            return source[key];
        }

        /// <summary>
        ///     Get value associated with the specified key of dictionary if it exists, else add a new pair of key and value to
        ///     dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value">Value that will be added to dictionary, if it not contains specified key</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            where TKey : notnull
        {
            if (source.TryGetValue(key, out var existedValue)) return existedValue;

            source.Add(key, value);
            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="GetOrAdd{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory">Function that gets value that must be added to dictionary, if it not contains specified key</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key,
            Func<TKey, TValue> valueFactory)
            where TKey : notnull
        {
            if (source.TryGetValue(key, out var existedValue)) return existedValue;

            source.Add(key, valueFactory(key));
            return source[key];
        }

        /// <summary>
        ///     <inheritdoc cref="GetOrAdd{TKey,TValue}(System.Collections.Generic.IDictionary{TKey,TValue},TKey,TValue)" />
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory">Function that gets value that must be added to dictionary, if it not contains specified key</param>
        /// <param name="valueFactoryArgument">Additional parameter for factory function <paramref name="valueFactory" /></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue, TArg>(this IDictionary<TKey, TValue> source, TKey key,
            Func<TKey, TArg, TValue> valueFactory, TArg valueFactoryArgument)
            where TKey : notnull
        {
            if (source.TryGetValue(key, out var existedValue)) return existedValue;

            source.Add(key, valueFactory(key, valueFactoryArgument));
            return source[key];
        }

        /// <summary>
        ///     Transforms collection of key-value pairs to dictionary with abstract value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<TKey, object?> ToAbstractDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
            where TKey : notnull
        {
            return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as object);
        }

        /// <summary>
        ///     Add collection of key-value pairs to dictionary, skipping values associated with already existing keys
        /// </summary>
        /// <typeparam name="TDictionary"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static TDictionary TryAddRange<TDictionary, TKey, TValue>(this TDictionary source,
            IEnumerable<KeyValuePair<TKey, TValue>> other)
            where TDictionary : IDictionary<TKey, TValue>
            where TKey : notnull
        {
            foreach (var (key, value) in other) source.TryAdd(key, value);
            return source;
        }

        /// <summary>
        ///     Add collection of key-value pairs to dictionary, updating values associated with already existing keys
        /// </summary>
        /// <typeparam name="TDictionary"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static TDictionary TryAddOrUpdateRange<TDictionary, TKey, TValue>(this TDictionary source,
            IEnumerable<KeyValuePair<TKey, TValue>> other)
            where TDictionary : IDictionary<TKey, TValue>
            where TKey : notnull
        {
            foreach (var (key, value) in other) source.AddOrUpdate(key, value);
            return source;
        }
    }
}
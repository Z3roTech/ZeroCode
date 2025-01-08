using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extension methods for classes that implements <see cref="IDisposable" /> or <see cref="IAsyncDisposable" />
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        ///     Execute dispose method for <paramref name="target" /> if <paramref name="target" /> implement
        ///     <see cref="IAsyncDisposable" /> or <see cref="IDisposable" />
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static async ValueTask DisposeIfPossibleAsync(this object? target)
        {
            switch (target)
            {
                case null:
                    return;

                case IAsyncDisposable disposable:
                    await disposable.DisposeAsync();
                    return;

                case IDisposable disposable:
                    disposable.Dispose();
                    return;
            }
        }

        /// <inheritdoc cref="DisposeIfPossibleAsync" />
        public static void DisposeIfPossible(this object? target)
        {
            DisposeIfPossibleAsync(target).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Execute <see cref="IDisposable.Dispose()" /> for all elements of sequence consequently.
        /// </summary>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDisposable> DisposeAll<TDisposable>(this List<TDisposable> source)
            where TDisposable : IDisposable
        {
            source.ForEach(disposable => disposable.Dispose());
            return source;
        }

        /// <summary>
        ///     Execute <see cref="IDisposable.Dispose()" /> for all elements of sequence consequently.
        /// </summary>
        /// <typeparam name="TCollection">Any class that implements <see cref="IEnumerable{T}" /></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TCollection DisposeAll<TCollection>(this TCollection source)
            where TCollection : IEnumerable<IDisposable>
        {
            foreach (var disposable in source) disposable.Dispose();
            return source;
        }

        /// <summary>
        ///     Asynchronously execute <see cref="IAsyncDisposable.DisposeAsync()" /> for all elements of sequence consequently.
        /// </summary>
        /// <typeparam name="TCollection">Any class that implements <see cref="IEnumerable{T}" /></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<TCollection> DisposeAllAsync<TCollection>(this TCollection source)
            where TCollection : IEnumerable<IAsyncDisposable>
        {
            foreach (var disposable in source) await disposable.DisposeAsync();
            return source;
        }
    }
}
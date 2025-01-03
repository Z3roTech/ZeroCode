using System;
using System.Threading.Tasks;

namespace ZeroCode.Extensions.Common
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
    }
}
using System;
using System.Threading;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extensions methods for <see cref="CancellationTokenSource" />.
    /// </summary>
    public static class CancellationExtensions
    {
        /// <summary>
        ///     Communicates a request for <see cref="CancellationTokenSource" /> if cancellation not requested already
        /// </summary>
        /// <param name="cts">Cancellation token source that must trigger cancellation</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CancelIfPossible(this CancellationTokenSource cts)
        {
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            if (cts.IsCancellationRequested) return;

            cts.Cancel();
        }

        /// <inheritdoc cref="CancelIfPossible(System.Threading.CancellationTokenSource)" />
        /// <param name="throwOnFirstException">
        ///     <inheritdoc cref="CancellationTokenSource.Cancel(bool)" />
        /// </param>
        public static void CancelIfPossible(this CancellationTokenSource cts, bool throwOnFirstException)
        {
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            if (cts.IsCancellationRequested) return;

            cts.Cancel(throwOnFirstException);
        }

        /// <summary>
        ///     Schedule cancel operation on this <see cref="CancellationTokenSource" /> after specified time delay if cancellation
        ///     not requested already
        /// </summary>
        /// <param name="cts">Cancellation token source that must trigger cancellation</param>
        /// <param name="delay">
        ///     <inheritdoc cref="CancellationTokenSource.CancelAfter(System.TimeSpan)" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CancelAfterIfPossible(this CancellationTokenSource cts, TimeSpan delay)
        {
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            if (cts.IsCancellationRequested) return;

            cts.CancelAfter(delay);
        }

        /// <summary>
        ///     <inheritdoc cref="CancelAfterIfPossible(System.Threading.CancellationTokenSource,System.TimeSpan)" />
        /// </summary>
        /// <param name="cts">Cancellation token source that must trigger cancellation</param>
        /// <param name="millisecondsDelay">
        ///     <inheritdoc cref="CancellationTokenSource.CancelAfter(int)" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CancelAfterIfPossible(this CancellationTokenSource cts, int millisecondsDelay)
        {
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            if (cts.IsCancellationRequested) return;

            cts.CancelAfter(millisecondsDelay);
        }
    }
}
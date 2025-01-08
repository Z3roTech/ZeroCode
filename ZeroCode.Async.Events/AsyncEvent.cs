using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroCode.Async
{
    /// <summary>
    ///     Internal methods for invoking async delegates like event
    /// </summary>
    internal static class AsyncEvent
    {
        /// <summary>
        ///     Internal method for invoking event delegates with specific invocation mode
        /// </summary>
        /// <param name="event">Event handler that will be invoked</param>
        /// <param name="mode">Invocation mode of <paramref name="event" /></param>
        /// <param name="sender">Object, event invoked from</param>
        /// <param name="eventArgs">Event arguments that must be sent to delegates</param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static async Task InvokeAsyncInternal(
            AsyncEventHandler @event,
            AsyncEventInvocationMode mode,
            object? sender,
            EventArgs? eventArgs,
            CancellationToken token
        )
        {
            var invokes = @event
                .GetInvocationList()
                .Cast<AsyncEventHandler>()
                .ToArray();

            if (invokes.Length == 0) return;

            switch (mode)
            {
                case AsyncEventInvocationMode.Consequently:
                    foreach (var handler in invokes)
                        await handler.Invoke(sender, eventArgs, token).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.Simultaneously:
                    await Task.WhenAll(
                        invokes.Select(handler =>
                            Task.Run(() => handler.Invoke(sender, eventArgs, token), CancellationToken.None))
                    ).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.TrueAsync:
                    await Task.WhenAll(
                        invokes.Select(handler => handler.Invoke(sender, eventArgs, token))
                    );

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <inheritdoc cref="InvokeAsyncInternal" />
        /// <typeparam name="TEventArgs">Class with data that send to delegate</typeparam>
        internal static async Task InvokeAsyncInternal<TEventArgs>(
            AsyncEventHandler<TEventArgs> @event,
            AsyncEventInvocationMode mode,
            object? sender,
            TEventArgs eventArgs,
            CancellationToken token
        )
        {
            var invokes = @event
                .GetInvocationList()
                .Cast<AsyncEventHandler<TEventArgs>>()
                .ToArray();

            if (invokes.Length == 0) return;

            switch (mode)
            {
                case AsyncEventInvocationMode.Consequently:
                    foreach (var handler in invokes)
                        await handler.Invoke(sender, eventArgs, token).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.Simultaneously:
                    await Task.WhenAll(
                        invokes.Select(handler =>
                            Task.Run(() => handler.Invoke(sender, eventArgs, token), CancellationToken.None))
                    ).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.TrueAsync:
                    await Task.WhenAll(
                        invokes.Select(handler => handler.Invoke(sender, eventArgs, token))
                    );

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <inheritdoc cref="InvokeAsyncInternal" />
        /// <typeparam name="TEventSender">Sender class type</typeparam>
        /// <typeparam name="TEventArgs">Event arguments class type</typeparam>
        internal static async Task InvokeAsyncInternal<TEventSender, TEventArgs>(
            AsyncEventHandler<TEventSender, TEventArgs> @event,
            AsyncEventInvocationMode mode,
            TEventSender sender,
            TEventArgs eventArgs,
            CancellationToken token
        )
        {
            var invokes = @event
                .GetInvocationList()
                .Cast<AsyncEventHandler<TEventSender, TEventArgs>>()
                .ToArray();

            if (invokes.Length == 0) return;

            switch (mode)
            {
                case AsyncEventInvocationMode.Consequently:
                    foreach (var handler in invokes)
                        await handler.Invoke(sender, eventArgs, token).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.Simultaneously:
                    await Task.WhenAll(
                        invokes.Select(handler =>
                            Task.Run(() => handler.Invoke(sender, eventArgs, token), CancellationToken.None))
                    ).ConfigureAwait(false);

                    break;

                case AsyncEventInvocationMode.TrueAsync:
                    await Task.WhenAll(
                        invokes.Select(handler => handler.Invoke(sender, eventArgs, token))
                    );

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
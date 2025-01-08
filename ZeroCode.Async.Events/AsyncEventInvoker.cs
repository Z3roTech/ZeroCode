using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroCode.Async
{
    /// <summary>
    ///     Invoker for <see cref="AsyncEventHandler" /> delegate
    /// </summary>
    internal class AsyncEventInvoker : IAsyncEventInvoker<object?, EventArgs?>
    {
        private readonly AsyncEventHandler _event;

        public AsyncEventInvoker(AsyncEventHandler @event)
        {
            _event = @event;
        }

        /// <inheritdoc />
        public Task Consequently(object? sender, EventArgs? args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Consequently, sender, args, token);
        }

        /// <inheritdoc />
        public Task Simultaneously(object? sender, EventArgs? args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Simultaneously, sender, args, token);
        }

        /// <inheritdoc />
        public Task TrueAsynchronously(object? sender, EventArgs? args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.TrueAsync, sender, args, token);
        }
    }

    /// <summary>
    ///     Invoker for <see cref="AsyncEventHandler{TEventArgs}" /> delegate
    /// </summary>
    internal class AsyncEventInvoker<TEventArgs> : IAsyncEventInvoker<object?, TEventArgs>
    {
        private readonly AsyncEventHandler<TEventArgs> _event;

        public AsyncEventInvoker(AsyncEventHandler<TEventArgs> @event)
        {
            _event = @event;
        }

        /// <inheritdoc />
        public Task Consequently(object? sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Consequently, sender, args, token);
        }

        /// <inheritdoc />
        public Task Simultaneously(object? sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Simultaneously, sender, args, token);
        }

        /// <inheritdoc />
        public Task TrueAsynchronously(object? sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.TrueAsync, sender, args, token);
        }
    }

    /// <summary>
    ///     Invoker for <see cref="AsyncEventHandler{TEventSender, TEventArgs}" /> delegate
    /// </summary>
    internal class AsyncEventInvoker<TEventSender, TEventArgs> : IAsyncEventInvoker<TEventSender, TEventArgs>
    {
        private readonly AsyncEventHandler<TEventSender, TEventArgs> _event;

        public AsyncEventInvoker(AsyncEventHandler<TEventSender, TEventArgs> @event)
        {
            _event = @event;
        }

        /// <inheritdoc />
        public Task Consequently(TEventSender sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Consequently, sender, args, token);
        }

        /// <inheritdoc />
        public Task Simultaneously(TEventSender sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.Simultaneously, sender, args, token);
        }

        /// <inheritdoc />
        public Task TrueAsynchronously(TEventSender sender, TEventArgs args, CancellationToken token)
        {
            return AsyncEvent.InvokeAsyncInternal(_event, AsyncEventInvocationMode.TrueAsync, sender, args, token);
        }
    }
}
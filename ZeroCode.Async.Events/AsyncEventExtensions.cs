using System;

namespace ZeroCode.Async
{
    /// <summary>
    ///     Extensions that makes async event invocation possible
    /// </summary>
    public static class AsyncEventExtensions
    {
        /// <summary>
        ///     Create async invoker for <paramref name="event" />. Use that invoker methods for invoke event in specified mode.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static IAsyncEventInvoker<object?, EventArgs?> InvokeAsync(this AsyncEventHandler @event)
        {
            return new AsyncEventInvoker(@event);
        }

        /// <inheritdoc cref="InvokeAsync" />
        public static IAsyncEventInvoker<object?, TEventArgs> InvokeAsync<TEventArgs>(
            this AsyncEventHandler<TEventArgs> @event)
        {
            return new AsyncEventInvoker<TEventArgs>(@event);
        }

        /// <inheritdoc cref="InvokeAsync" />
        /// s
        public static IAsyncEventInvoker<TEventSender, TEventArgs> InvokeAsync<TEventSender, TEventArgs>(
            this AsyncEventHandler<TEventSender, TEventArgs> @event)
        {
            return new AsyncEventInvoker<TEventSender, TEventArgs>(@event);
        }
    }
}
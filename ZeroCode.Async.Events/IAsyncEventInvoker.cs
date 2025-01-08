using System.Threading;
using System.Threading.Tasks;

namespace ZeroCode.Async
{
    /// <summary>
    ///     Interface for all async event invokers
    /// </summary>
    /// <typeparam name="TEventSender"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    public interface IAsyncEventInvoker<in TEventSender, in TEventArgs>
    {
        /// <summary>
        ///     Invoke event consequently, awaiting completion of task before running next.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Consequently(TEventSender sender, TEventArgs args, CancellationToken token);

        /// <summary>
        ///     Invoke event simultaneously, run all delegates in dedicated tasks and then await its competition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Simultaneously(TEventSender sender, TEventArgs args, CancellationToken token);

        /// <summary>
        ///     Invoke event as true asynchronously work in .NET. Invoke delegate until it return async context, after that invoke
        ///     next, in the end await competition of all tasks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task TrueAsynchronously(TEventSender sender, TEventArgs args, CancellationToken token);
    }
}
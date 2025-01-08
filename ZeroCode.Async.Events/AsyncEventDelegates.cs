using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroCode.Async
{
    /// <summary>
    ///     Async event handler delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public delegate Task AsyncEventHandler(
        object? sender,
        EventArgs? args,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Async event handler delegate with specifying sending event args type
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public delegate Task AsyncEventHandler<in TEventArgs>(
        object? sender,
        TEventArgs args,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Async event handler delegate with specifying sender and sending event args types
    /// </summary>
    /// <typeparam name="TEventSender"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public delegate Task AsyncEventHandler<in TEventSender, in TEventArgs>(
        TEventSender sender,
        TEventArgs args,
        CancellationToken cancellationToken = default
    );
}
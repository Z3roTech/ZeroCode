namespace ZeroCode.Async.Events
{
    /// <summary>
    ///     Invocation mods of async events
    /// </summary>
    internal enum AsyncEventInvocationMode
    {
        /// <summary>
        ///     Await result of delegate before invoke next.
        /// </summary>
        Consequently,

        /// <summary>
        ///     Create dedicated Task for each delegate and await until completion of all tasks.
        /// </summary>
        Simultaneously,

        /// <summary>
        ///     Start invoking of delegate until it return async context, after that invoke next. In the end await completion of
        ///     all tasks.
        /// </summary>
        TrueAsync
    }
}
using System;
using System.Threading;

namespace ZeroCode
{
    /// <summary>
    ///     Helper that can be used to prevent repeated invoke <see cref="IDisposable.Dispose" />
    /// </summary>
    public class OneTimeDisposingHelper
    {
        private int _disposingFlag;

        /// <summary>
        ///     Dependent object is already tried to dispose
        /// </summary>
        public bool IsDisposed => _disposingFlag != 0;

        /// <summary>
        ///     Check this trying to dispose is first time or repeat
        /// </summary>
        /// <returns></returns>
        public bool TryDispose()
        {
            return Interlocked.CompareExchange(ref _disposingFlag, 1, 0) == 0;
        }
    }
}
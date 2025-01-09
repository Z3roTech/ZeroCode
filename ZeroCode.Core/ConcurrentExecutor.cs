using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroCode
{
    /// <summary>
    ///     Helper class for invoking methods in concurrent mode. It may help to queue requests to database on application
    ///     layer instead of database layer.
    /// </summary>
    public class ConcurrentExecutor
    {
        /// <summary>
        ///     Synchronization primitive that will be control concurrent methods invoke
        /// </summary>
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        ///     Count of maximum tasks of invoking methods that can be executed in meantime
        /// </summary>
        private int _maxParallelTasks;

        public ConcurrentExecutor(int maxParallelTasks)
        {
            if (maxParallelTasks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxParallelTasks),
                    "Value of maximum parallel executing tasks must be more then 0");

            _maxParallelTasks = maxParallelTasks;
            _semaphore = new SemaphoreSlim(_maxParallelTasks);
        }

        /// <summary>
        ///     Change count of maximum methods that will be invoking in meantime. This method can be executing too long when
        ///     lowering count of maximum parallel tasks.
        /// </summary>
        /// <param name="maxParallelTasks"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ConcurrentExecutor ChangeMaxParallelTaskCount(int maxParallelTasks)
        {
            if (maxParallelTasks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxParallelTasks),
                    "Value of maximum parallel executing tasks must be more then 0");

            var oldValue = Interlocked.Exchange(ref _maxParallelTasks, maxParallelTasks);
            var diff = maxParallelTasks - oldValue;

            // If value of max parallel now more than previous, we must release new slots in semaphore
            if (diff > 0) _semaphore.Release(diff);

            // If value of max parallel tasks now less than previous, we must use semaphore slots
            if (diff < 0)
                for (var i = 0; i < Math.Abs(diff); i++)
                    _semaphore.Wait();

            return this;
        }

        /// <summary>
        ///     Execute in concurrent mode <paramref name="wrapped" />
        /// </summary>
        /// <typeparam name="TResult">Any result type</typeparam>
        /// <param name="wrapped">Method that must be invoked in concurrent mode</param>
        /// <param name="timeout">Timeout of waiting invoking in concurrent queue</param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TimeoutException">If execution was timed out</exception>
        public TResult Execute<TResult>(Func<TResult> wrapped, TimeSpan timeout = default,
            CancellationToken token = default)
        {
            if (wrapped == null) throw new ArgumentNullException(nameof(wrapped));

            try
            {
                if (timeout == TimeSpan.Zero)
                    _semaphore.Wait(token);
                else if (!_semaphore.Wait(timeout, token)) throw new TimeoutException();

                return wrapped();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc cref="Execute{TResult}" />
        public void Execute(Action wrapped, TimeSpan timeout = default,
            CancellationToken token = default)
        {
            if (wrapped == null) throw new ArgumentNullException(nameof(wrapped));

            try
            {
                if (timeout == TimeSpan.Zero)
                    _semaphore.Wait(token);
                else if (!_semaphore.Wait(timeout, token)) throw new TimeoutException();

                wrapped();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc cref="Execute{TResult}" />
        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> wrapped, TimeSpan timeout = default,
            CancellationToken token = default)
        {
            if (wrapped == null) throw new ArgumentNullException(nameof(wrapped));

            try
            {
                if (timeout == TimeSpan.Zero)
                    await _semaphore.WaitAsync(token);
                else if (!await _semaphore.WaitAsync(timeout, token)) throw new TimeoutException();

                return await wrapped();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc cref="Execute{TResult}" />
        public async Task ExecuteAsync(Func<Task> wrapped, TimeSpan timeout = default,
            CancellationToken token = default)
        {
            if (wrapped == null) throw new ArgumentNullException(nameof(wrapped));

            try
            {
                if (timeout == TimeSpan.Zero)
                    await _semaphore.WaitAsync(token);
                else if (!await _semaphore.WaitAsync(timeout, token)) throw new TimeoutException();

                await wrapped();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    /// <summary>
    ///     Static cache of <see cref="ConcurrentExecutor" />
    /// </summary>
    public static class ConcurrentExecutorCache
    {
        /// <summary>
        ///     Cache of all created named executors
        /// </summary>
        private static readonly ConcurrentDictionary<string, ConcurrentExecutor> Executors =
            new ConcurrentDictionary<string, ConcurrentExecutor>();

        /// <summary>
        ///     Returns existing or created new named executor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxParallelTasks"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ConcurrentExecutor GetOrCreate(string name, int maxParallelTasks)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return Executors.GetOrAdd(name, new ConcurrentExecutor(maxParallelTasks));
        }

        /// <summary>
        ///     Clear cache of named executors
        /// </summary>
        public static void ClearExecutors()
        {
            Executors.Clear();
        }
    }
}
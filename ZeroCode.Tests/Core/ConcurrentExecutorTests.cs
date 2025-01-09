namespace ZeroCode.Tests.Core;

public class ConcurrentExecutorTests
{
    [TearDown]
    public void TearDownTest()
    {
        ConcurrentExecutorCache.ClearExecutors();
    }

    [Test]
    public async Task ExecuteMoreThanMaximumTest()
    {
        var executor = ConcurrentExecutorCache.GetOrCreate(nameof(ConcurrentExecutorTests), 2);
        _ = executor.ExecuteAsync(async () => await Task.Delay(500).ConfigureAwait(false));
        _ = executor.ExecuteAsync(async () => await Task.Delay(500).ConfigureAwait(false));
        Assert.Throws<TimeoutException>(() => { executor.Execute(() => { }, TimeSpan.FromMilliseconds(50)); });

        await Task.Delay(500);
        Assert.DoesNotThrowAsync(() => executor.ExecuteAsync(() => Task.CompletedTask, TimeSpan.FromMilliseconds(50)));
    }

    [Test]
    public async Task ChangeMaximumParallelTasksTest()
    {
        var executor = ConcurrentExecutorCache.GetOrCreate(nameof(ConcurrentExecutorTests), 2);
        _ = executor.ExecuteAsync(async () => await Task.Delay(500).ConfigureAwait(false));
        _ = executor.ExecuteAsync(async () => await Task.Delay(500).ConfigureAwait(false));

        // it takes some time
        executor.ChangeMaxParallelTaskCount(1);
        await Task.Delay(100);

        _ = executor.ExecuteAsync(async () => await Task.Delay(500).ConfigureAwait(false));
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await executor.ExecuteAsync(() => Task.CompletedTask, TimeSpan.FromMilliseconds(50));
        });
    }
}
using ZeroCode.Extensions.Common;

namespace ZeroCode.Tests.ExtensionsTest;

public class CancellationsTests
{
    [Test]
    public void CancelIfPossibleTest()
    {
        var cancels = 0;
        using var alreadyCanceledCts = new CancellationTokenSource();
        using var alreadyCanceledCtsReg = alreadyCanceledCts.Token.Register(() =>
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Interlocked.Increment(ref cancels);
        });

        alreadyCanceledCts.Cancel();

        alreadyCanceledCts.CancelIfPossible();
        using var notCancelledCts = new CancellationTokenSource();
        using var notCancelledCtsReg = notCancelledCts.Token.Register(() =>
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Interlocked.Increment(ref cancels);
        });

        notCancelledCts.CancelIfPossible();

        Assert.That(cancels, Is.EqualTo(2));
    }

    [Test]
    public async Task CancelAfterIfPossibleTest()
    {
        var cancels = 0;
        using var alreadyCanceledCts = new CancellationTokenSource();
        await using var alreadyCanceledCtsReg = alreadyCanceledCts.Token.Register(() =>
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Interlocked.Increment(ref cancels);
        });

        await alreadyCanceledCts.CancelAsync();

        alreadyCanceledCts.CancelAfterIfPossible(100);
        using var notCancelledCts = new CancellationTokenSource();
        await using var notCancelledCtsReg = notCancelledCts.Token.Register(() =>
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Interlocked.Increment(ref cancels);
        });

        notCancelledCts.CancelIfPossible();

        await Task.Delay(1_000, CancellationToken.None);
        Assert.That(cancels, Is.EqualTo(2));
    }
}
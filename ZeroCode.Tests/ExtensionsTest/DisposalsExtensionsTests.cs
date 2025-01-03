using ZeroCode.Extensions.Common;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.ExtensionsTest;

public class DisposalsExtensionsTests
{
    [Test]
    public void DisposeIfPossibleTest()
    {
        object asyncDisposable = new TestingAllDisposableClass();
        asyncDisposable.DisposeIfPossible();
        Assert.That(
            asyncDisposable,
            Has.Property(nameof(TestingAllDisposableClass.DisposalType))
                .EqualTo(TestingAllDisposableClass.DisposedAsyncronosly)
        );

        object disposable = new TestingDisposableClass();
        disposable.DisposeIfPossible();
        Assert.That(
            disposable,
            Has.Property(nameof(TestingDisposableClass.DisposalType))
                .EqualTo(TestingDisposableClass.DisposedSyncronosly)
        );
    }
}
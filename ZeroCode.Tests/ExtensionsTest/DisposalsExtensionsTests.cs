using ZeroCode.Extensions;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.ExtensionsTest;

public class DisposalsExtensionsTests
{
    [Test]
    public void DisposeIfPossibleTest()
    {
        object asyncDisposable = new TestingBothDisposableClass();
        asyncDisposable.DisposeIfPossible();
        Assert.That(
            asyncDisposable,
            Has.Property(nameof(TestingBothDisposableClass.DisposalType))
                .EqualTo(TestingBothDisposableClass.DisposedAsynchronously)
        );

        object disposable = new TestingDisposableClass();
        disposable.DisposeIfPossible();
        Assert.That(
            disposable,
            Has.Property(nameof(TestingDisposableClass.DisposalType))
                .EqualTo(TestingDisposableClass.DisposedSynchronously)
        );
    }

    [Test]
    public async Task DisposeAllTest()
    {
        var disposableList = new List<TestingDisposableClass>
        {
            new(), new(), new(), new()
        };

        disposableList.DisposeAll();
        Assert.That(disposableList, Has.All
            .Property(nameof(TestingDisposableClass.DisposalType))
            .EqualTo(TestingDisposableClass.DisposedSynchronously));

        var disposableEnumerable = new List<TestingDisposableClass>
        {
            new(), new(), new(), new()
        }.AsEnumerable();

        disposableEnumerable.DisposeAll();
        Assert.That(disposableEnumerable, Has.All
            .Property(nameof(TestingDisposableClass.DisposalType))
            .EqualTo(TestingDisposableClass.DisposedSynchronously));

        var asyncDisposableCollection = new List<TestingAsyncDisposableClass>
        {
            new(), new(), new(), new()
        }.AsEnumerable();

        await asyncDisposableCollection.DisposeAllAsync();


        Assert.That(asyncDisposableCollection, Has.All
            .Property(nameof(TestingAsyncDisposableClass.DisposalType))
            .EqualTo(TestingAsyncDisposableClass.DisposedAsynchronously));
    }
}
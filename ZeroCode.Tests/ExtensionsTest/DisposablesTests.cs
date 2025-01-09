using ZeroCode.Extensions;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.ExtensionsTest;

public class DisposablesTests
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

    [Test]
    public void OneTimeDisposingHelperTest()
    {
        // Dispose not safe for multiple invoke
        var disposableObject = new TestingDisposableClass();
        disposableObject.Dispose();
        disposableObject.Dispose();
        disposableObject.Dispose();
        Assert.That(disposableObject, Has.Property(nameof(disposableObject.DisposalType)).EqualTo(3));

        var dh = new OneTimeDisposingHelper();
        disposableObject = new TestingDisposableClass();
        DisposeObject(dh, disposableObject);
        DisposeObject(dh, disposableObject);
        DisposeObject(dh, disposableObject);
        Assert.That(dh, Has.Property(nameof(dh.IsDisposed)).True);
        Assert.That(disposableObject,
            Has.Property(nameof(disposableObject.DisposalType))
                .EqualTo(TestingDisposableClass.DisposedSynchronously)); // => 1

        return;

        void DisposeObject(OneTimeDisposingHelper helper, IDisposable disposable)
        {
            if (!helper.TryDispose()) return;

            disposable.Dispose();
        }
    }

    [Test]
    public void GenericDisposableTest()
    {
        var list = new List<TestingClass>();
        var unsubscribe1st = Subscribe(TestingClass.CreateRandomNew());
        var unsubscribe2nd = SubscribeWithState(TestingClass.CreateRandomNew());

        Assert.That(list, Has.Count.EqualTo(2));
        unsubscribe1st.Dispose();
        unsubscribe1st.Dispose(); // no errors
        unsubscribe1st.Dispose(); // no errors
        Assert.That(list, Has.Count.EqualTo(1));
        unsubscribe2nd.Dispose();
        Assert.That(list, Is.Empty);

        return;
        IDisposable Subscribe(TestingClass subscriber)
        {
            list.Add(subscriber);
            return new GenericDisposable(() => list.Remove(subscriber));
        }

        IDisposable SubscribeWithState(TestingClass subscriber)
        {
            list.Add(subscriber);
            return new GenericDisposable((sub) => list.Remove((TestingClass)sub!), subscriber);
        }
    }
}
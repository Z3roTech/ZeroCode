using System.Collections.Concurrent;
using ZeroCode.Async.Events;
using ZeroCode.Tests.Moq;

namespace ZeroCode.Tests.AsyncEvents;

public sealed class SimultaneouslyEventsInvokeTests
{
    private static readonly object[] TestingCaseDataForDefaultEventHandler =
    [
        new object[] { new[] { 800, 400, 200, 600, 1000 }, new[] { 3, 2, 4, 1, 5 } },
        new object[] { new[] { 200, 400, 600, 800, 1000 }, new[] { 1, 2, 3, 4, 5 } },
        new object[] { new[] { 1000, 800, 600, 400, 200 }, new[] { 5, 4, 3, 2, 1 } },
        new object[] { new[] { 100, 500, 200, 600, 300 }, new[] { 1, 3, 5, 2, 4 } }
    ];

    private readonly ConcurrentQueue<int> _finishedTasksIds = new();
    private event AsyncEventHandler? DefaultEvents;
    private event AsyncEventHandler<TestingEventArgs>? EventsWithCustomEventArgs;
    private event AsyncEventHandler<TestingClass, TestingEventArgs>? EventsWithCustomSenderAndArgs;

    [SetUp]
    public void PrepareTest()
    {
        _finishedTasksIds.Clear();
    }

    [TearDown]
    public void DisposeTest()
    {
        DefaultEvents = null;
        EventsWithCustomEventArgs = null;
        EventsWithCustomSenderAndArgs = null;
    }

    [TestCaseSource(nameof(TestingCaseDataForDefaultEventHandler))]
    public async Task TestingDefault(int[] delays, int[] expectedOrder)
    {
        var senderMoq = new object();
        var eventArgsMoq = EventArgs.Empty;
        for (var i = 0; i < delays.Length; i++)
        {
            var index = i;
            DefaultEvents += (sender, args, token) => EventMethod(index + 1, delays[index], sender, args, token);
        }

        await DefaultEvents!
            .InvokeAsync()
            .Simultaneously(senderMoq, eventArgsMoq, CancellationToken.None)
            .ConfigureAwait(false);

        AssertExpectedAndActualOrder(expectedOrder, _finishedTasksIds.ToArray());

        return;

        async Task EventMethod(int id, int delay, object? sender, EventArgs? args, CancellationToken cancellationtoken)
        {
            await Task.Delay(delay, cancellationtoken).ConfigureAwait(false);
            _finishedTasksIds.Enqueue(id);

            Assert.That(sender, Is.EqualTo(senderMoq));
            Assert.That(args, Is.EqualTo(eventArgsMoq));
        }
    }

    [TestCaseSource(nameof(TestingCaseDataForDefaultEventHandler))]
    public async Task TestingWithCustomArgs(int[] delays, int[] expectedOrder)
    {
        var senderMoq = new object();
        var eventArgsMoq = TestingEventArgs.RandomNew();
        for (var i = 0; i < delays.Length; i++)
        {
            var index = i;
            EventsWithCustomEventArgs += (sender, args, token) =>
                EventMethod(index + 1, delays[index], sender, args, token);
        }

        await EventsWithCustomEventArgs!
            .InvokeAsync()
            .Simultaneously(senderMoq, eventArgsMoq, CancellationToken.None)
            .ConfigureAwait(false);

        AssertExpectedAndActualOrder(expectedOrder, _finishedTasksIds.ToArray());

        return;

        async Task EventMethod(int id, int delay, object? sender, TestingEventArgs? args,
            CancellationToken cancellationtoken)
        {
            await Task.Delay(delay, cancellationtoken).ConfigureAwait(false);
            _finishedTasksIds.Enqueue(id);

            Assert.That(sender, Is.EqualTo(senderMoq));
            Assert.That(args, Is.EqualTo(eventArgsMoq));
            Assert.That(args.Id, Is.EqualTo(eventArgsMoq.Id));
        }
    }

    [TestCaseSource(nameof(TestingCaseDataForDefaultEventHandler))]
    public async Task TestingWithCustomSenderAndArgs(int[] delays, int[] expectedOrder)
    {
        var senderMoq = new TestingClass { Guid = Guid.NewGuid(), Id = Random.Shared.Next() };
        var eventArgsMoq = TestingEventArgs.RandomNew();
        for (var i = 0; i < delays.Length; i++)
        {
            var index = i;
            EventsWithCustomSenderAndArgs += (sender, args, token) =>
                EventMethod(index + 1, delays[index], sender, args, token);
        }

        await EventsWithCustomSenderAndArgs!
            .InvokeAsync()
            .Simultaneously(senderMoq, eventArgsMoq, CancellationToken.None)
            .ConfigureAwait(false);

        AssertExpectedAndActualOrder(expectedOrder, _finishedTasksIds.ToArray());

        return;

        async Task EventMethod(int id, int delay, TestingClass? sender, TestingEventArgs? args,
            CancellationToken cancellationtoken)
        {
            await Task.Delay(delay, cancellationtoken).ConfigureAwait(false);
            _finishedTasksIds.Enqueue(id);

            Assert.That(sender, Is.EqualTo(senderMoq));
            Assert.That(sender.Id, Is.EqualTo(senderMoq.Id));
            Assert.That(sender.Guid, Is.EqualTo(senderMoq.Guid));

            Assert.That(args, Is.EqualTo(eventArgsMoq));
            Assert.That(args.Id, Is.EqualTo(eventArgsMoq.Id));
        }
    }

    private void AssertExpectedAndActualOrder(int[] expectedOrder, int[] actualOrder)
    {
        Assert.That(expectedOrder, Is.Not.Null.And.Not.Empty);
        Assert.That(actualOrder, Is.Not.Null.And.Not.Empty);

        Assert.That(expectedOrder, Has.Length.EqualTo(actualOrder.Length));
        Assert.That(expectedOrder, Is.EquivalentTo(actualOrder));

        for (var i = 0; i < actualOrder.Length; i++) Assert.That(expectedOrder[i], Is.EqualTo(actualOrder[i]));
    }
}
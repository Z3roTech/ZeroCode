namespace ZeroCode.Tests.Moq;

public class TestingEventArgs : EventArgs
{
    public new static readonly TestingEventArgs Empty = new() { Id = Guid.Empty };
    public Guid Id { get; init; }

    public static TestingEventArgs RandomNew()
    {
        return new TestingEventArgs { Id = Guid.NewGuid() };
    }
}
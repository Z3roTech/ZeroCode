namespace ZeroCode.Tests.Moq;

public class TestingClass
{
    public int Id { get; init; }
    public Guid Guid { get; init; }

    public static TestingClass CreateRandomNew()
    {
        return new TestingClass
        {
            Guid = Guid.NewGuid(),
            Id = Random.Shared.Next()
        };
    }
}

public class TestingBothDisposableClass : IDisposable, IAsyncDisposable
{
    public const int DisposedAsynchronously = 2;
    public const int DisposedSynchronously = 1;
    private int _disposalType;
    public int DisposalType => _disposalType;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        Interlocked.Add(ref _disposalType, DisposedAsynchronously);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Interlocked.Add(ref _disposalType, DisposedSynchronously);
    }
}

public class TestingDisposableClass : IDisposable
{
    public const int DisposedSynchronously = 1;
    private int _disposalType;
    public int DisposalType => _disposalType;

    /// <inheritdoc />
    public void Dispose()
    {
        Interlocked.Add(ref _disposalType, DisposedSynchronously);
    }
}

public class TestingAsyncDisposableClass : IAsyncDisposable
{
    public const int DisposedAsynchronously = 2;
    private int _disposalType;
    public int DisposalType => _disposalType;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        Interlocked.Add(ref _disposalType, DisposedAsynchronously);
    }
}

public enum TestingEnum
{
    [System.ComponentModel.Description("Description of Foo")]
    Foo,

    [System.ComponentModel.Description("Description of Bar")]
    Bar
}
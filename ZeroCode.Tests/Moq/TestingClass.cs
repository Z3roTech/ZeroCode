namespace ZeroCode.Tests.Moq;

public class TestingClass
{
    public int Id { get; init; }
    public Guid Guid { get; init; }
}

public class TestingAllDisposableClass : IDisposable, IAsyncDisposable
{
    public const int DisposedAsyncronosly = 2;
    public const int DisposedSyncronosly = 1;
    private int _disposalType;
    public int DisposalType => _disposalType;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        Interlocked.Add(ref _disposalType, DisposedAsyncronosly);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Interlocked.Add(ref _disposalType, DisposedSyncronosly);
    }
}

public class TestingDisposableClass : IDisposable
{
    public const int DisposedSyncronosly = 1;
    private int _disposalType;
    public int DisposalType => _disposalType;

    /// <inheritdoc />
    public void Dispose()
    {
        Interlocked.Add(ref _disposalType, DisposedSyncronosly);
    }
}
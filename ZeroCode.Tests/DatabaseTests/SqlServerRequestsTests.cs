using System.Data;
using Microsoft.Data.SqlClient;
using ZeroCode.Database.SqlServer;

namespace ZeroCode.Tests.DatabaseTests;

public class SqlServerRequestsTests
{
    private const string ConnectionString =
        "Server=ZEROPC;Integrated Security=true;Encrypt=false;";

    private const string QueryWithOutput = "select * from [NullDb].[dbo].[TestTable] (nolock) where Id > @id";
    private const string QueryWithoutOutput = "delete from [NullDb].[dbo].[TestTable] where Id = @id";

    private static readonly RequestBody RequestBodyForQueryWithOutput =
        new(QueryWithOutput, CommandType.Text,
            new Dictionary<string, object?> { ["id"] = 0 });

    private static readonly RequestBody RequestBodyForQueryWithoutOutput =
        new(QueryWithoutOutput, CommandType.Text,
            new Dictionary<string, object?> { ["id"] = 0 });

    private SqlConnection? _connection;

    [OneTimeSetUp]
    public void InitTests() { }

    [SetUp]
    public void PrepareTest() { }

    [OneTimeTearDown]
    public void FinalizeTests() { }

    [TearDown]
    public void TearDownTest()
    {
        _connection?.Dispose();
        _connection = null;
        Request.ClearGlobalConnectionString();
    }

    [Test]
    public void TryUsingRequestMethodsTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Request.SetGlobalConnectionString(null!));
        Assert.Throws<ArgumentOutOfRangeException>(() => Request.SetGlobalConnectionString(string.Empty));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.ExecuteAsync(new RequestBody("select 1", CommandType.Text), default(SqlConnection)!,
                CancellationToken.None));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.ExecuteAsync(new RequestBody("select 1", CommandType.Text), default(string)!,
                CancellationToken.None));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await Request.ExecuteAsync(new RequestBody("select 1", CommandType.Text), CancellationToken.None));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.ExecuteNonQueryAsync(new RequestBody("select 1", CommandType.Text), default(SqlConnection)!,
                CancellationToken.None));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.ExecuteNonQueryAsync(new RequestBody("select 1", CommandType.Text), default(string)!,
                CancellationToken.None));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await Request.ExecuteNonQueryAsync(new RequestBody("select 1", CommandType.Text), CancellationToken.None));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.Batch.ExecuteAsync(
                [new RequestBody("select 1", CommandType.Text)],
                default(SqlConnection)!,
                CancellationToken.None
            ));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await Request.Batch.ExecuteAsync(
                [new RequestBody("select 1", CommandType.Text)],
                default(string)!,
                CancellationToken.None
            ));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await Request.Batch.ExecuteAsync(
                [new RequestBody("select 1", CommandType.Text)],
                CancellationToken.None
            ));
    }

    private static void AssertTestSelect(Dictionary<string, object?>[][] result)
    {
        Assert.That(result, Is.Not.Null.And.Not.Empty);
        var table = result[0];
        Assert.That(table, Is.Not.Null.And.Not.Empty);
        Assert.That(table, Has.Length.EqualTo(3954));

        foreach (var row in table)
            Assert.Multiple(() =>
            {
                Assert.That(row.ContainsKey("Id"), Is.True);
                Assert.That(row.ContainsKey("SomeString"), Is.True);
                Assert.That(row["Id"], Is.Not.Null);
            });
    }

    [Test]
    public async Task MakeQueryViaConnectionStringTest()
    {
        var result = await Request.ExecuteAsync(
            RequestBodyForQueryWithOutput,
            ConnectionString,
            CancellationToken.None
        );

        AssertTestSelect(result);

        await Request.ExecuteNonQueryAsync(
            RequestBodyForQueryWithoutOutput,
            ConnectionString,
            CancellationToken.None
        );
    }

    [Test]
    public async Task MakeQueryViaConnectionInstanceTest()
    {
        _connection = new SqlConnection(ConnectionString);
        var result = await Request.ExecuteAsync(
            RequestBodyForQueryWithOutput,
            _connection,
            CancellationToken.None
        );

        AssertTestSelect(result);

        await Request.ExecuteNonQueryAsync(
            RequestBodyForQueryWithoutOutput,
            _connection,
            CancellationToken.None
        );
    }

    [Test]
    public async Task MakeQueryViaGlobalConnectionStringTest()
    {
        Request.SetGlobalConnectionString(ConnectionString);
        var result = await Request.ExecuteAsync(
            RequestBodyForQueryWithOutput,
            CancellationToken.None
        );

        AssertTestSelect(result);

        await Request.ExecuteNonQueryAsync(
            RequestBodyForQueryWithoutOutput,
            CancellationToken.None
        );
    }

    [Test]
    public async Task MakeBatchQueriesViaConnectionStringTest()
    {
        var allResults = await Request.Batch.ExecuteAsync(
            [RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput],
            ConnectionString,
            CancellationToken.None
        );

        Assert.That(allResults, Is.Not.Null.And.Not.Empty);
        foreach (var result in allResults) AssertTestSelect(result);

        await Request.Batch.ExecuteNonQueryAsync(
            [RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput],
            ConnectionString,
            CancellationToken.None
        );
    }

    [Test]
    public async Task MakeBatchQueriesViaConnectionInstanceTest()
    {
        _connection = new SqlConnection(ConnectionString);
        var allResults = await Request.Batch.ExecuteAsync(
            [RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput],
            _connection,
            CancellationToken.None
        );

        Assert.That(allResults, Is.Not.Null.And.Not.Empty);
        foreach (var result in allResults) AssertTestSelect(result);

        await Request.Batch.ExecuteNonQueryAsync(
            [RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput],
            _connection,
            CancellationToken.None
        );
    }

    [Test]
    public async Task MakeBatchQueriesViaGlobalConnectionStringTest()
    {
        Request.SetGlobalConnectionString(ConnectionString);
        var allResults = await Request.Batch.ExecuteAsync(
            [RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput, RequestBodyForQueryWithOutput],
            CancellationToken.None
        );

        Assert.That(allResults, Is.Not.Null.And.Not.Empty);
        foreach (var result in allResults) AssertTestSelect(result);

        await Request.Batch.ExecuteNonQueryAsync(
            [RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput, RequestBodyForQueryWithoutOutput],
            CancellationToken.None
        );
    }
}
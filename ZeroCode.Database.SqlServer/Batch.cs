using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ZeroCode.Database.SqlServer
{
    public static partial class Request
    {
        /// <summary>
        ///     Helper class for executing multiple queries consequently
        /// </summary>
        public static class Batch
        {
            /// <summary>
            ///     Execute multiple query requests without output
            /// </summary>
            /// <param name="requests"></param>
            /// <param name="connectionString"></param>
            /// <param name="connection">An existing connection that can be used for executing query</param>
            /// <param name="useTransaction">
            ///     If <see langword="true" /> execution will be covered in transaction, that will be
            ///     rollbacked on exception.
            /// </param>
            /// <param name="token"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            internal static async Task ExecuteNonQueryAsyncInternal(
                RequestBody[] requests,
                string? connectionString,
                SqlConnection? connection,
                bool useTransaction = false,
                CancellationToken token = default
            )
            {
                var controllableDispose = connection != null;
                connection ??= new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);
                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

                var transaction = useTransaction
                    ? await connection.BeginTransactionAsync(token)
                    : null;

                try
                {
                    foreach (var body in requests)
                        await Request.ExecuteNonQueryAsyncInternal(body, connectionString, connection, token);

                    if (transaction != null) await transaction.CommitAsync(token);
                }
                catch (OperationCanceledException)
                {
                    if (transaction != null) await transaction.RollbackAsync(CancellationToken.None);
                    throw;
                }
                catch
                {
                    if (transaction != null) await transaction.RollbackAsync(token);
                    throw;
                }
                finally
                {
                    if (!controllableDispose)
                    {
                        if (connection.State != ConnectionState.Closed) await connection.CloseAsync();
                        await connection.DisposeAsync();
                    }
                }
            }

            /// <summary>
            ///     Execute multiple query requests with outputed tabled values
            /// </summary>
            /// <param name="requests"></param>
            /// <param name="connectionString"></param>
            /// <param name="connection">An existing connection that can be used for executing query</param>
            /// <param name="useTransaction">
            ///     If <see langword="true" /> execution will be covered in transaction, that will be
            ///     rollbacked on exception.
            /// </param>
            /// <param name="token"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            internal static async Task<List<Dictionary<string, object?>[][]>> ExecuteAsyncInternal(
                RequestBody[] requests,
                string? connectionString,
                SqlConnection? connection,
                bool useTransaction = false,
                CancellationToken token = default
            )
            {
                var controllableDispose = connection != null;
                connection ??= new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);
                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

                var transaction = useTransaction
                    ? await connection.BeginTransactionAsync(token)
                    : null;

                try
                {
                    var result = await requests.ToAsyncEnumerable()
                        .SelectAwaitWithCancellation(async (body, cancellationToken) =>
                            await Request.ExecuteAsyncInternal(body, connectionString, connection, token))
                        .ToListAsync(token);

                    if (transaction != null) await transaction.CommitAsync(token);

                    return result;
                }
                catch (OperationCanceledException)
                {
                    if (transaction != null) await transaction.RollbackAsync(CancellationToken.None);
                    throw;
                }
                catch
                {
                    if (transaction != null) await transaction.RollbackAsync(token);
                    throw;
                }
                finally
                {
                    if (!controllableDispose)
                    {
                        if (connection.State != ConnectionState.Closed) await connection.CloseAsync();
                        await connection.DisposeAsync();
                    }
                }
            }

            /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
            public static Task ExecuteNonQueryAsync(
                RequestBody[] requests,
                string connectionString,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

                return ExecuteNonQueryAsyncInternal(requests, connectionString, null, useTransaction, token);
            }

            /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
            public static Task ExecuteNonQueryAsync(
                RequestBody[] requests,
                SqlConnection connection,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                return ExecuteNonQueryAsyncInternal(requests, null, connection, useTransaction, token);
            }

            /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
            public static Task ExecuteNonQueryAsync(
                RequestBody[] requests,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (_globalConnectionString == null)
                    throw new InvalidOperationException(
                        ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                    );

                return ExecuteNonQueryAsyncInternal(requests, _globalConnectionString, null, useTransaction, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                string connectionString,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

                return ExecuteAsyncInternal(requests, connectionString, null, useTransaction, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                SqlConnection connection,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                return ExecuteAsyncInternal(requests, null, connection, useTransaction, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                bool useTransaction = false,
                CancellationToken token = default)
            {
                if (_globalConnectionString == null)
                    throw new InvalidOperationException(
                        ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                    );

                return ExecuteAsyncInternal(requests, _globalConnectionString, null, useTransaction, token);
            }
        }
    }
}
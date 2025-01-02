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
            /// <param name="token"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            internal static async Task ExecuteNonQueryAsyncInternal(
                RequestBody[] requests,
                string? connectionString,
                SqlConnection? connection,
                CancellationToken token = default
            )
            {
                var controllableDispose = connection != null;
                connection ??= new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);
                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

                try
                {
                    foreach (var body in requests)
                        await Request.ExecuteNonQueryAsyncInternal(body, connectionString, connection, token);
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
            /// <param name="token"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            internal static async Task<List<Dictionary<string, object?>[][]>> ExecuteAsyncInternal(
                RequestBody[] requests,
                string? connectionString,
                SqlConnection? connection,
                CancellationToken token = default
            )
            {
                var controllableDispose = connection != null;
                connection ??= new SqlConnection(connectionString);
                if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);
                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

                try
                {
                    var result = await requests.ToAsyncEnumerable()
                        .SelectAwaitWithCancellation(async (body, cancellationToken) =>
                            await Request.ExecuteAsyncInternal(body, connectionString, connection, token))
                        .ToListAsync(token);

                    return result;
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
                CancellationToken token = default)
            {
                if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

                return ExecuteNonQueryAsyncInternal(requests, connectionString, null, token);
            }

            /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
            public static Task ExecuteNonQueryAsync(
                RequestBody[] requests,
                SqlConnection connection,
                CancellationToken token = default)
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                return ExecuteNonQueryAsyncInternal(requests, null, connection, token);
            }

            /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
            public static Task ExecuteNonQueryAsync(
                RequestBody[] requests,
                CancellationToken token = default)
            {
                if (_globalConnectionString == null)
                    throw new InvalidOperationException(
                        ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                    );

                return ExecuteNonQueryAsyncInternal(requests, _globalConnectionString, null, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                string connectionString,
                CancellationToken token = default)
            {
                if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

                return ExecuteAsyncInternal(requests, connectionString, null, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                SqlConnection connection,
                CancellationToken token = default)
            {
                if (connection == null) throw new ArgumentNullException(nameof(connection));

                return ExecuteAsyncInternal(requests, null, connection, token);
            }

            /// <inheritdoc cref="ExecuteAsyncInternal" />
            public static Task<List<Dictionary<string, object?>[][]>> ExecuteAsync(
                RequestBody[] requests,
                CancellationToken token = default)
            {
                if (_globalConnectionString == null)
                    throw new InvalidOperationException(
                        ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                    );

                return ExecuteAsyncInternal(requests, _globalConnectionString, null, token);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ZeroCode.Database.SqlServer
{
    /// <summary>
    ///     Utility funtions for executing query in database
    /// </summary>
    public static partial class Request
    {
        /// <summary>
        ///     Connection string to database that uses when nor connection string nor openned connection sent with query
        /// </summary>
        private static string? _globalConnectionString;

        /// <summary>
        ///     Setter for global connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetGlobalConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentOutOfRangeException(nameof(connectionString));

            _globalConnectionString = connectionString;
        }

        /// <summary>
        ///     Execute query without output
        /// </summary>
        /// <param name="queryBody"></param>
        /// <param name="connectionString"></param>
        /// <param name="connection">An existing connection that can be used for executing query</param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static async Task ExecuteNonQueryAsyncInternal(
            RequestBody queryBody,
            string? connectionString,
            SqlConnection? connection,
            CancellationToken token = default)
        {
            var controllableDispose = connection != null;
            connection ??= new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);

            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

            try
            {
                await using var cmd = new SqlCommand(queryBody.Query, connection);
                cmd.CommandType = queryBody.QueryType;

                if (queryBody.QueryParams != null)
                    cmd.Parameters.AddRange(
                        queryBody.QueryParams
                            .Select(p => new SqlParameter(p.Key, p.Value))
                            .ToArray()
                    );

                await cmd.ExecuteNonQueryAsync(token);
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
        ///     Execute query with output
        /// </summary>
        /// <param name="queryBody"></param>
        /// <param name="connectionString"></param>
        /// <param name="connection">An existing connection that can be used for executing query</param>
        /// <param name="token"></param>
        /// <returns>Task with an array of an arrays (rows of table) of dictionry (column of a row)</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static async Task<Dictionary<string, object?>[][]> ExecuteAsyncInternal(
            RequestBody queryBody,
            string? connectionString = null,
            SqlConnection? connection = null,
            CancellationToken token = default)
        {
            var controllableDispose = connection != null;
            connection ??= new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync(token);

            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException(ExceptionsHelper.CantOpenSqlConnection);

            try
            {
                await using var cmd = new SqlCommand(queryBody.Query, connection);
                cmd.CommandType = queryBody.QueryType;

                if (queryBody.QueryParams != null)
                    cmd.Parameters.AddRange(
                        queryBody.QueryParams
                            .Select(p => new SqlParameter(p.Key, p.Value))
                            .ToArray()
                    );

                using var adapter = new SqlDataAdapter(cmd);
                using var dataset = new DataSet();
                adapter.Fill(dataset);

                return dataset.Tables
                    .Cast<DataTable>()
                    .Select(table => new
                    {
                        Headers = table.Columns.Cast<DataColumn>().Select(column => column.ColumnName),
                        Table = table
                    })
                    .Select(arg => arg.Table.Rows
                        .Cast<DataRow>()
                        .Select(row => arg.Headers.ToDictionary(header => header, header => row?[header]))
                        .ToArray()
                    )
                    .ToArray();
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
        public static Task ExecuteNonQueryAsync(RequestBody queryBody, string connectionString,
            CancellationToken token = default)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            return ExecuteNonQueryAsyncInternal(queryBody, connectionString, null, token);
        }

        /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
        public static Task ExecuteNonQueryAsync(RequestBody queryBody, SqlConnection connection,
            CancellationToken token = default)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            return ExecuteNonQueryAsyncInternal(queryBody, null, connection, token);
        }

        /// <inheritdoc cref="ExecuteNonQueryAsyncInternal" />
        public static Task ExecuteNonQueryAsync(RequestBody queryBody, CancellationToken token = default)
        {
            if (_globalConnectionString == null)
                throw new InvalidOperationException(
                    ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                );

            return ExecuteNonQueryAsyncInternal(queryBody, _globalConnectionString, null, token);
        }

        /// <inheritdoc cref="ExecuteAsyncInternal" />
        public static Task<Dictionary<string, object?>[][]> ExecuteAsync(RequestBody queryBody, string connectionString,
            CancellationToken token = default)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            return ExecuteAsyncInternal(queryBody, connectionString, null, token);
        }

        /// <inheritdoc cref="ExecuteAsyncInternal" />
        public static Task<Dictionary<string, object?>[][]> ExecuteAsync(RequestBody queryBody,
            SqlConnection connection,
            CancellationToken token = default)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            return ExecuteAsyncInternal(queryBody, null, connection, token);
        }

        /// <inheritdoc cref="ExecuteAsyncInternal" />
        public static Task<Dictionary<string, object?>[][]> ExecuteAsync(RequestBody queryBody,
            CancellationToken token = default)
        {
            if (_globalConnectionString == null)
                throw new InvalidOperationException(
                    ExceptionsHelper.UseConnectionStringSetterFirst(nameof(SetGlobalConnectionString))
                );

            return ExecuteAsyncInternal(queryBody, _globalConnectionString, null, token);
        }
    }
}
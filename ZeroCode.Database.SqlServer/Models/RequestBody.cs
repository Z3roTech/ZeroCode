using System.Collections.Generic;
using System.Data;

// ReSharper disable once CheckNamespace
namespace ZeroCode.Database
{
    /// <summary>
    ///     Database query request body for using in <see cref="Request" /> methods
    /// </summary>
    public class RequestBody
    {
        public RequestBody(
            string query,
            CommandType queryType,
            IReadOnlyDictionary<string, object?>? queryParams = null)
        {
            Query = query;
            QueryType = queryType;
            QueryParams = queryParams;
        }

        /// <summary>
        ///     SQL Query string, name of stored procedure or table name
        /// </summary>
        public string Query { get; }

        /// <summary>
        ///     Type of query, that written in <see cref="Query" />
        /// </summary>
        public CommandType QueryType { get; }

        /// <summary>
        ///     Query params (named variables) for query when <see cref="QueryType" /> is
        ///     <see cref="CommandType.StoredProcedure" /> or <see cref="CommandType.Text" />
        /// </summary>
        public IReadOnlyDictionary<string, object?>? QueryParams { get; }
    }
}
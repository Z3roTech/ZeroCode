namespace ZeroCode.Database.SqlServer
{
    /// <summary>
    ///     Some helper utils for throwing exceptions
    /// </summary>
    internal static class ExceptionsHelper
    {
        internal const string CantOpenSqlConnection = "Can't open SQL connection with database. Check your SQL string.";

        internal static string UseConnectionStringSetterFirst(string setterName)
        {
            return $"Set global connection string via \"{setterName}\" first.";
        }
    }
}
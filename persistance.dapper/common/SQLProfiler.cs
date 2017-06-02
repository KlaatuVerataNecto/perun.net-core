using MySql.Data.MySqlClient;
using StackExchange.Profiling;
using System.Data.Common;
using System.Data.SqlClient;

namespace persistance.dapper.common
{
    public class SQLProfiler
    {
        public static DbConnection GetOpenConnection(string connectionString)
        {
            var cnn = new MySqlConnection(connectionString);
            return new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler.Current);
        }
    }
}

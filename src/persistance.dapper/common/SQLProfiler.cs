using MySql.Data.MySqlClient;
using StackExchange.Profiling;
using System.Data.Common;

namespace persistance.dapper.common
{
    public class SQLProfiler
    {
        public static DbConnection GetOpenConnection(string connectionString)
        {
            return new MySqlConnection(connectionString); // that works OK         
            return new StackExchange.Profiling.Data.ProfiledDbConnection(
                new MySqlConnection(connectionString), MiniProfiler.Current
            );
        }
    }
}

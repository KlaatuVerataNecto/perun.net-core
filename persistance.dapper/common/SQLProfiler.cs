using System.Data.Common;
using System.Data.SqlClient;

namespace persistance.dapper.common
{
    public class SQLProfiler
    {
        public static DbConnection GetOpenConnection(string connectionString)
        {
            var cnn = new SqlConnection(connectionString); // A SqlConnection, SqliteConnection ... or whatever

            // wrap the connection with a profiling connection that tracks timings 
            //return StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler);
            return null;
        }
    }
}

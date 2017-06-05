namespace persistance.dapper.common
{
    public interface IDbConn
    {     string ConnectionString();
    }
    public interface IDapperService<T>
    {
    }
    public class DbConn : IDbConn
    {
        private string _conn;

        public DbConn(string conn)
        {
            _conn = conn;
        }

        public string ConnectionString()
        {
            return _conn;
        }
    }
    public abstract class DapperService<T> : IDapperService<T> where T : class
    {
        public readonly string _connectionString;

        public DapperService(IDbConn connection)
        {
            _connectionString = connection.ConnectionString();
        }
    }
}

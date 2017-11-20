using infrastucture.libs.providers;

namespace persistance.dapper.common
{
    public class DapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DapperConnectionFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IDapperConnection CreateConnection()
        {
            return new DapperConnection(_connectionStringProvider.ConnectionString);
        }
    }
}
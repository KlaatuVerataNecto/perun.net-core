namespace persistance.dapper.common
{
    public interface IDapperConnectionFactory
    {
        IDapperConnection CreateConnection();
    }
}

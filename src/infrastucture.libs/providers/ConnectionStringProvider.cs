namespace infrastucture.libs.providers
{ 
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; set; }
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString { get; set; }
    }
}

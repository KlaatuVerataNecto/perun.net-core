
namespace peruncore.Config
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool isSSL { get; set; }
        public string From { get; set; }
        public string Fromname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

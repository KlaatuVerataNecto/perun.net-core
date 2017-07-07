namespace infrastructure.email.models
{
    public class MailMessage
    {
        public string to { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public bool isSSL { get; set; }
        public string from { get; set; }
        public string fromname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string language { get; set; }
    }
}

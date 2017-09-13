namespace peruncore.Config
{
    public class AuthSchemeSettings
    {
        public string Application { get; set; }
        public string External { get; set; }
        public string Google { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public int ExpiryDays { get; set; }
    }
}

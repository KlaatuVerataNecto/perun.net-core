namespace persistance.ef.entity
{
    public class Login
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
	    public int external_id { get; set; }
	    public string provider { get; set; }
        public string access_token { get; set; }
        public string date_created { get; set; }
        public int user_id { get; set; }
        public User User { get; set; }
    }
}

using System;

namespace infrastructure.user.entities
{
    public class UserPassword
    {
        public int id { get; set; }
        public Login Login { get; set; }
        public int user_login_id { get; set; }  
        public string token { get; set; }
        public DateTime token_expiry_date { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
    }
}

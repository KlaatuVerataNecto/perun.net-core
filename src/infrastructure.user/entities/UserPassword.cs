using System;

namespace infrastructure.user.entities
{
    public class UserPassword
    {
        public Login Login { get; set; }
        public int user_login_id { get; set; }
        public DateTime date_created { get; set; }    

        public string password_token { get; set; }
        public DateTime password_token_expiry_date { get; set; }

        //public string newemail { get; set; }
        //public string newemail_token { get; set; }
        //public DateTime newemail_token_expiry_date { get; set; }
    }
}

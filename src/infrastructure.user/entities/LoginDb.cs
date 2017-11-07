using System;
using System.Collections.Generic;

namespace infrastructure.user.entities
{
    public class LoginDb
    {
        public int id { get; set; }
        public string email { get; set; }
        public string passwd { get; set; }
        public string salt { get; set; }
	    public string external_id { get; set; }
	    public string provider { get; set; }
        public string access_token { get; set; }
        public DateTime date_created { get; set; }
        public int user_id { get; set; }
        public UserDb User { get; set; }
        public ICollection<UserPasswordDb> UserPasswordResets { get; set; }
        public ICollection<UserEmailDb> UserEmailChanges { get; set; }
    }
}

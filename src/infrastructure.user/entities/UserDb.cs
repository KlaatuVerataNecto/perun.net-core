using System;
using System.Collections.Generic;

namespace infrastructure.user.entities
{
    public class UserDb
    {
        public int id { get; set; }
        public string username { get; set; }
        public string roles { get; set; }
        public DateTime last_seen { get; set; }
        public DateTime date_created { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public bool is_locked { get; set; }
        public ICollection<LoginDb> Logins { get; set; }
        public UserUsernameDb UsernameToken { get; set; }
        
    }
}

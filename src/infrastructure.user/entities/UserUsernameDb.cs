using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.user.entities
{
    public class UserUsernameDb
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string token { get; set; }
        public DateTime date_created { get; set; }

        public UserDb User { get; set; }
    }
}

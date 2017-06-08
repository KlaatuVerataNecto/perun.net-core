using System;
using System.Collections.Generic;

namespace persistance.ef.entity
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string roles { get; set; }
        public DateTime last_seen { get; set; }
        public DateTime date_created { get; set; }
        public string avatar { get; set; }
        public bool is_locked { get; set; }
        public ICollection<Login> Logins { get; set; }
    }
}

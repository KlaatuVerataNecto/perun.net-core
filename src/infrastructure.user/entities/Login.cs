﻿using System;
using System.Collections.Generic;

namespace infrastructure.user.entities
{
    public class Login
    {
        public int id { get; set; }
        public string email { get; set; }
        public string passwd { get; set; }
        public string salt { get; set; }
	    public int? external_id { get; set; }
	    public string provider { get; set; }
        public string access_token { get; set; }
        public DateTime date_created { get; set; }
        public int user_id { get; set; }
        public User User { get; set; }
        public ICollection<UserPassword> UserPasswordResets { get; set; }
        public ICollection<UserEmail> UserEmailChanges { get; set; }
    }
}

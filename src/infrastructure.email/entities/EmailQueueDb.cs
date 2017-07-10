using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.email.entities
{
    public class EmailQueueDb
    {   
        public int id { get; set; }
        public string receiver { get; set; }
        public string sender { get; set; }
        public string email_body { get; set; }
        public string email_subject { get; set; }
        public DateTime date_sent { get; set; }
        public bool is_sent { get; set; }
    }
}

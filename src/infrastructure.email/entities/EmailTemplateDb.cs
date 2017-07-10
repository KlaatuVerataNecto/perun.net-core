
namespace infrastructure.email.entities
{
    public class EmailTemplateDb
    {
        public int id { get; set; }
        public string sender_email { get; set; }
        public string sender_name { get; set; }
        public string email_body { get; set; }
        public string email_subject { get; set; }
        public string template_type { get; set; }
    }
}

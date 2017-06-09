using infrastructure.libs.validators;

namespace infrastructure.user.models
{
    public class UserAuthDB
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string roles { get; set; }
        public string avatar { get; set; }
        public string provider { get; set; }
        public string salt { get; set; }
    }
}

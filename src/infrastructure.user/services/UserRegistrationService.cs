
using System;

namespace infrastructure.user.services
{
    public interface IUserRegistrationService
    {
        bool is_username_available(string username, int user_id_to_skip);
        bool is_email_available(string username, int user_id_to_skip);
    }

    public class UserRegistrationService : IUserRegistrationService
    {
        public bool is_username_available(string username, int user_id_to_skip)
        {
            throw new NotImplementedException();
        }
        public bool is_email_available(string email, int user_id_to_skip)
        {
            throw new NotImplementedException();
        }
    }
}

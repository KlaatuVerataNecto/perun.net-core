using System.Collections.Generic;
using infrastructure.user.models;

namespace infrastructure.user.interfaces
{   public interface IUserRepository
    {
        UserAuthDB GetByEmail(string email);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        UserAuthDB Add(string username, string email, string hashed_password, string salt, string provider);
    }
}

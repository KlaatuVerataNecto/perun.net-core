using infrastructure.user.entities;

namespace infrastructure.user.interfaces
{   public interface IUserRepository
    {
        Login GetByEmail(string email);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        Login AddLogin(string username, string email, string hashed_password, string salt, string provider);
        Login AddUserPassword(string username, string email, string hashed_password, string salt, string provider);
    }
}

using infrastructure.user.entities;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        Login GetByEmail(string email);
        Login GetByEmailWithResetInfo(string email);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        Login AddLogin(Login obj);
        void UpdateLogin(Login obj);
    }
}

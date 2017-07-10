using infrastructure.user.entities;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        LoginDb GetByEmail(string email);
        LoginDb GetByEmailWithResetInfo(string email);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        LoginDb AddLogin(LoginDb obj);
        void UpdateLogin(LoginDb obj);
    }
}

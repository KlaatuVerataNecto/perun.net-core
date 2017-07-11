using infrastructure.user.entities;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        LoginDb getByEmail(string email);
        LoginDb getByEmailWithResetInfo(string email);
        LoginDb getByIdWithResetInfo(int id);
        bool isUsernameAvailable(string username);
        bool isEmailAvailable(string email);
        LoginDb addLogin(LoginDb obj);
        void updateLogin(LoginDb obj);
    }
}

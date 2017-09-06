using infrastructure.user.entities;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        LoginDb getByIdWithUserNameToken(int id);
        LoginDb getByEmailAndProvider(string email, string provider);
        LoginDb getByEmailWithResetInfo(string email, string provider);
        LoginDb getByIdWithResetInfo(int id, string provider);
        bool isUsernameAvailable(string username);
        bool isEmailAvailable(string email);
        LoginDb addLogin(LoginDb obj);
        void updateLogin(LoginDb obj);
    }
}

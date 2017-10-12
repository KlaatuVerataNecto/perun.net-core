using infrastructure.user.entities;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        UserDb getUserById(int id);
        LoginDb getByIdWithUserNameToken(int id);
        LoginDb getByEmailAndProvider(string email, string provider);        
        LoginDb getIdAndProvider(int id, string provider);
        LoginDb getByEmailWithResetInfo(string email, string provider);
        LoginDb getByIdWithResetInfo(int id, string provider);
        List<LoginDb> getLoginsByUserId(int id);

        LoginDb getById(int id);
        bool isUsernameAvailable(string username);
        bool isEmailAvailable(string email);
        LoginDb addLogin(LoginDb obj);
        void updateLogin(LoginDb obj);
        void updateUser(UserDb obj);
    }
}

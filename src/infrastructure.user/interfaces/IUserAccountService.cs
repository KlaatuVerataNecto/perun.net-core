using infrastructure.user.models;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        string changeUsername(int userid, string username);
        string changeAvatar(int userid, string avatar);
        UserIdentity changeUsernameByToken(int userid, string username, string token);
        UserLogin getApplicationLoginById(int userid);
        UserUsername getUsernameByUserId(int userid);
        EmailChange createEmailChangeRequest(int userId, string password, string newEmail, int tokenLength, int expiryDays);
        EmailChanged applyEmailByToken(int userId, string token);
        void cancelEmailActivation(int userId);
        string getPendingNewEmailActivation(int loginId);
        List<UserLogin> getLoginsByUserId(int userid);
        bool changePassword(int userid, string currentPassowrd, string newPassowrd, int saltLength);
    }
}

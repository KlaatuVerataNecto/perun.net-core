using infrastructure.user.models;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        UserIdentity changeUsername(int userid, string username, string token);
        EmailChange createEmailChangeRequest(int userId, string password, string newEmail, int tokenLength, int expiryDays);
        string verifyEmailChangeToken(int userId, string token);
        void cancelEmailActivation(int userId);
        string getPendingNewEmailActivation(int loginId);
        List<UserLogin> getLoginsByUserId(int userid);
        bool changePassword(int userid, string currentPassowrd, string newPassowrd, int saltLength);
    }
}

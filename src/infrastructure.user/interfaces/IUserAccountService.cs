using infrastructure.user.models;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        UserIdentity changeUsername(int userid, string username, string token);
        EmailChange createEmailChangeRequest(int userid, string password, string newemail, int tokenLength, int expiryDays);
        List<UserLogin> getLoginsByUserId(int userid);
    }
}

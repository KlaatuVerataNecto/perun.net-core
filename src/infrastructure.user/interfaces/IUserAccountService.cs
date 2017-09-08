using infrastructure.user.models;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        UserIdentity changeUsername(int userid, string username, string token);
        List<UserLogin> getLoginsByUserId(int userid);
    }
}

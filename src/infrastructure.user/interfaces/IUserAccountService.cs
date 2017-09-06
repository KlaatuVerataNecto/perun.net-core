using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        UserIdentity ChangeUsername(int userid, string username, string token);
    }
}

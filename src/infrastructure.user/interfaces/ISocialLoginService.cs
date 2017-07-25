using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface ISocialLoginService
    {
        UserIdentity loginOrSignup(string nameIdentifier, string email, string firstname, string lastname, string provider);
        UsernameToken getTokenByUserId(int userid);
    }
}

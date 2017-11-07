
using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface IUserRegistrationService
    {
        UserIdentity signup(string username, string email, string password, string provider, int saltLength);
    }
}

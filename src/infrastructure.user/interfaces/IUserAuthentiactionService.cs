using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface IUserAuthentiactionService
    {
        UserIdentity login(string email, string password);
    }
}

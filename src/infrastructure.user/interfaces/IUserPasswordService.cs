using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface IUserPasswordService
    {
        UserReset generateResetToken(string email, int tokenLength, int expiryDays);
    }
}

using infrastructure.user.models;

namespace infrastructure.user.interfaces
{
    public interface IUserPasswordService
    {
        UserReset generateResetToken(string email, int tokenLength, int expiryDays);
        UserReset verifyToken(int userId, string token);
        UserReset changePassword(int userid, string token, string password);
    }
}

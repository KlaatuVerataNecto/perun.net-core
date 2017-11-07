using infrastructure.libs.validators;

namespace infrastructure.user.models
{
    public class UserUsername
    {
        private readonly int _userid;
        private readonly string _username;

        public UserUsername(int userid, string username)
        {
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(username, "username is invalid.");

            _userid = userid;
            _username = username;
        }

        public int UserId => _userid;
        public string Username => _username;
    }
}

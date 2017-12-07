using infrastructure.libs.validators;

namespace infrastructure.user.models
{
    public class UserProfile
    {
        private readonly int _userid;
        private readonly string _username;
        private readonly string _avatar;

        public UserProfile(int userid, string username, string avatar)
        {
            CustomValidators.StringNotNullorEmpty(username, "username is required.");
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            _username = username;
            _userid = userid;
            _avatar = avatar;
        }

        public string Avatar => _avatar;
        public string Username => _username;
        public int UserId => _userid;
    }
}

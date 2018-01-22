using infrastructure.libs.validators;

namespace infrastructure.user.models
{
    public class UserProfile
    {
        private readonly int _userid;
        private readonly string _username;
        private readonly string _avatar;
        private readonly string _cover;

        public UserProfile(int userid, string username, string avatar, string cover)
        {
            CustomValidators.StringNotNullorEmpty(username, "username is required.");
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            _username = username;
            _userid = userid;
            _avatar = avatar;
            _cover = cover;
        }

        public string Avatar => _avatar;
        public string Cover => _cover;
        public string Username => _username;
        public int UserId => _userid;
    }
}

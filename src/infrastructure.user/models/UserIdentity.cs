using infrastructure.libs.validators;

namespace infrastructure.user.models
{
    public class UserIdentity
    {
        private readonly int _userid;
        private readonly string _username;
        private readonly string _email;
        private readonly string _roles;
        private readonly string _avatar;
        private readonly string _loginProvider;
        private readonly bool _isRequiresNewUsername;

        public UserIdentity(int userid, string username, string email, string loginProvider, string roles, string avatar, bool isRequiresNewUsername = false)
        {
            CustomValidators.StringNotNullorEmpty(username, "username is required.");
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(email, "email is invalid.");
            CustomValidators.StringNotNullorEmpty(loginProvider, "login_provider is invalid.");

            _username = username;
            _userid = userid;
            _email = email;
            _roles = roles;
            _loginProvider = loginProvider;
            _avatar = avatar;
            _isRequiresNewUsername = isRequiresNewUsername;
        }

        public string Avatar => _avatar;
        public string LoginProvider => _loginProvider;   
        public string Email => _email;
        public string Username => _username;
        public int UserId => _userid;
        public string Roles => _roles;
        public bool IsRequiresNewUsername => _isRequiresNewUsername;

    }
}
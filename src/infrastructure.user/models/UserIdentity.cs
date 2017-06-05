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
        private readonly string _loginprovider;

        public UserIdentity(int userid, string username, string email, string loginProvider, string roles, string avatar)
        {
            CustomValidators.StringNotNullorEmpty(username, "username is required.");
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(email, "email is invalid.");
            CustomValidators.StringNotNullorEmpty(loginProvider, "login_provider is invalid.");

            _username = username;
            _userid = userid;
            _email = email;
            _roles = roles;
            _loginprovider = loginProvider;
            _avatar = avatar;
        }

        public string Avatar
        {
            get { return _avatar; }
        }

        public string LoginProvider
        {
            get { return _loginprovider; }
        }

        public string Email
        {
            get { return _email; }
        }

        public string Username
        {
            get { return _username; }
        }

        public int UserId
        {
            get { return _userid; }
        }

        public string Roles
        {
            get { return _roles; }
        }
    }
}
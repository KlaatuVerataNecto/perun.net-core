using infrastructure.libs.validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.user.models
{
    public class UserLogin
    {
        private readonly int _userid;
        private readonly string _email;
        private readonly string _username;
        private readonly string _provider;
        private readonly bool _isdefault;

        public UserLogin(int userid, string username, string email, string provider, string defaultProvider)
        {
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(username, "username is invalid.");
            CustomValidators.StringNotNullorEmpty(email, "email is invalid.");
            CustomValidators.StringNotNullorEmpty(provider, "provider is invalid.");
            CustomValidators.StringNotNullorEmpty(defaultProvider, "defaultProvider is invalid.");

            _userid = userid;
            _username = username;
            _email = email;
            _provider = provider;
            _isdefault = defaultProvider == provider; 
        }

        public int UserId => _userid;
        public string Email => _email;
        public string Provider => _provider;
        public bool IsDefault => _isdefault;
        public string Username => _username;
    }
}

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
        private readonly string _provider;
        private readonly bool _isdefault;

        public UserLogin(int userid, string email, string provider, string defaultProvider )
        {
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(email, "email is invalid.");
            CustomValidators.StringNotNullorEmpty(provider, "provider is invalid.");

            _userid = userid;
            _email = email;
            _provider = provider;
            _isdefault = defaultProvider == provider; 
        }

        public int UserId => _userid;
        public string Email => _email;
        public string Provider => _provider;
        public bool IsDefault => _isdefault;
    }
}

using infrastructure.libs.validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure.user.models
{
    public class UsernameToken
    {
        private readonly int _userid;
        private readonly string _token;

        public UsernameToken(int userid, string token)
        {
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(token, "token is invalid.");

            _userid = userid;
            _token = token;
        }

        public int UserId => _userid;
        public string Token => _token;
    }
}

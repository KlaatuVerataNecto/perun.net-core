using infrastructure.libs.validators;
using System;

namespace infrastructure.user.models
{
    public class UserReset
    {
        private readonly int _userid;
        private readonly string _email;
        private readonly string _passwordToken;
        private readonly DateTime _passwordTokenExpiryDate;

        public UserReset(int userid, string email, string passwordToken, DateTime passwordTokenExpiryDate)
        {
            CustomValidators.IntNotNegative(userid, "userid is invalid.");
            CustomValidators.StringNotNullorEmpty(email, "email is invalid.");
            CustomValidators.StringNotNullorEmpty(passwordToken, "passwordToken is invalid.");
            CustomValidators.NotNull(passwordTokenExpiryDate, "passwordTokenExpiryDate is invalid.");

            _userid = userid;
            _email = email;
            _passwordToken = passwordToken;
            _passwordTokenExpiryDate = passwordTokenExpiryDate;
        }

        public int UserId => _userid;
        public string EmailTo => _email;
        public DateTime PasswordTokenExpiryDate => _passwordTokenExpiryDate;
        public string PasswordToken => _passwordToken;
    }
}

using infrastructure.libs.validators;
using System;

namespace infrastructure.user.models
{
    public class EmailChange
    {
        private readonly int _userId;
        private readonly string _currentEmail;
        private readonly string _newEmail;
        private readonly string _emailToken;
        private readonly DateTime _emailTokenExpiryDate;

        public EmailChange(int userId, string currentEmail, string newEmail, string emailToken, DateTime emailTokenExpiryDate)
        {
            CustomValidators.IntNotNegative(userId, "userId is invalid.");
            CustomValidators.StringNotNullorEmpty(currentEmail, "currentEmail is invalid.");
            CustomValidators.StringNotNullorEmpty(newEmail, "newEmail is invalid.");
            CustomValidators.StringNotNullorEmpty(emailToken, "emailToken is invalid.");
            CustomValidators.NotNull(emailTokenExpiryDate, "emailTokenExpiryDate is invalid.");

            _userId = userId;
            _currentEmail = currentEmail;
            _newEmail = newEmail;
            _emailToken = emailToken;
            _emailTokenExpiryDate = emailTokenExpiryDate;
        }

        public int UserId => _userId;
        public string EmailTo => _currentEmail;
        public DateTime EmailTokenExpiryDate => _emailTokenExpiryDate;
        public string EmailToken => _emailToken;
    }
}

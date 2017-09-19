using infrastructure.libs.validators;
using System;

namespace infrastructure.user.models
{
    public class EmailChanged
    {
        private readonly int _userId;
        private readonly string _currentEmail;
        private readonly string _oldEmail;


        public EmailChanged(int userId, string currentEmail, string oldEmail)
        {
            CustomValidators.IntNotNegative(userId, "userId is invalid.");
            CustomValidators.StringNotNullorEmpty(currentEmail, "currentEmail is invalid.");
            CustomValidators.StringNotNullorEmpty(oldEmail, "oldEmail is invalid.");


            _userId = userId;
            _currentEmail = currentEmail;
            _oldEmail = oldEmail;
        }

        public int UserId => _userId;
        public string OldEmail => _oldEmail;
        public string CurrentEmail => _currentEmail;
    }
}

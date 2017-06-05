using FluentValidation.Validators;
using infrastructure.i18n.user;
using System;
using System.Text.RegularExpressions;

namespace ui.web.Infrastructure.FluentValidators
{
    public class PasswordStrengthValidator : PropertyValidator
    {
        public PasswordStrengthValidator()
            : base(UserResponseMessagesResource.password_weak)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string password = context.PropertyValue as string;

            bool lower;
            bool upper;
            bool digit;
            bool digits;
            bool special;
            bool same;

            // test for NULL
            if (password == null)
                return false;


            if (String.IsNullOrEmpty(password))
                return false;

            if (password.Length < 6)
                return false;

            Match match = Regex.Match(password, @"[a-z]");
            lower = match.Success;

            // uncapitalize
            match = Regex.Match(password.Substring(0, 1).ToLower() + password.Substring(1), @"[A-Z]");
            upper = match.Success;

            match = Regex.Match(password, @"[0-9]");
            digit = match.Success;

            match = Regex.Match(password, @"[0-9].*[0-9]");
            digits = match.Success;

            match = Regex.Match(password, @"[^a-zA-Z0-9]");
            special = match.Success;

            match = Regex.Match(password, @"^(.)\1+$");
            same = match.Success;

            if (lower && upper && digit || lower && digits || upper && digits || special)
                return true;  //strong
            if (lower && upper || lower && digit || upper && digit)
                return true; //good
            return false; // weak
        }
    }
}
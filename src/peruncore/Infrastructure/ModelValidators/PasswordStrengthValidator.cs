using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace peruncore.Infrastructure.ModelValidators
{
    public class PasswordStrengthValidator : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string password = value as string;

            bool lower;
            bool upper;
            bool digit;
            bool digits;
            bool special;
            bool same;

            // test for NULL
            if (password == null)
                return new ValidationResult(base.ErrorMessage);


            if (String.IsNullOrEmpty(password))
                return new ValidationResult(base.ErrorMessage);

            if (password.Length < 6)
                return new ValidationResult(base.ErrorMessage);

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
                return ValidationResult.Success;  //strong
            if (lower && upper || lower && digit || upper && digit)
                return ValidationResult.Success; //good
            return new ValidationResult(base.ErrorMessage); // weak
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-passwordmeter", base.ErrorMessage);

        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value); return true;
        }

    }
}

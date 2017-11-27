using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace peruncore.Infrastructure.ModelValidators
{
    public class OnlyLettersDigitsUnderScoreValidator : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string str = value as string;
            if (value == null)
                return new ValidationResult(base.ErrorMessage);

            Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");

            if (regex.IsMatch(str))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(base.ErrorMessage);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-noweirdstuff", base.ErrorMessage);

        }

        private static bool MergeAttribute(IDictionary<string,string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value); return true;
        }

    }     
 }

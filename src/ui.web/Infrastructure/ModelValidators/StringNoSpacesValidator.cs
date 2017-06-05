using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ui.web.Infrastructure.ModelValidators
{
    public class StringNoSpacesValidator : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string str = value as string;
            if (value == null)
                return new ValidationResult("Nooooooo1!");

            if (str.Any(x => Char.IsWhiteSpace(x)))
            {
                return new ValidationResult("Noooooooooo2!");
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-nospaces", "Noooooooo3!");

        }

        private static bool MergeAttribute(IDictionary<string,string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value); return true;
        }

    }     
 }

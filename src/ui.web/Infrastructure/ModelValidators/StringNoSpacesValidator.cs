using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ui.web.Infrastructure.ModelValidators
{
    public class StringNoSpacesValidator : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string str = value as string;
            if (value == null)
                return new ValidationResult("Nooooooo!");

            if (str.Any(x => Char.IsWhiteSpace(x)))
            {
                return new ValidationResult("Noooooooooo!");
            }

            return ValidationResult.Success;
        }
    }
}

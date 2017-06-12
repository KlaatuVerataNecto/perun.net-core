//using FluentValidation;
//using FluentValidation.Validators;
//using infrastructure.i18n.user;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ui.web.Infrastructure.FluentValidators
//{
//    public class StringNoSpacesValidator : PropertyValidator
//    {
//        public StringNoSpacesValidator()
//            : base(UserResponseMessagesResource.username_empty_or_spaces)
//        {
//        }

//        protected override bool IsValid(PropertyValidatorContext context)
//        {
//            string value = context.PropertyValue as string;
//            if (value == null)
//                return false;

//            return !value.Any(x => Char.IsWhiteSpace(x));
//        }
//    }
//}
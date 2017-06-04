//using FluentValidation.Internal;
//using FluentValidation.Validators;
//using System.Collections.Generic;

//namespace ui.web.Infrastructure.FluentValidators
//{
//    public class PasswordStrengthPropertyValidator : FluentValidationPropertyValidator
//    {
//        public PasswordStrengthPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
//            : base(metadata, controllerContext, rule, validator)
//        {
//        }

//        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
//        {
//            if (!ShouldGenerateClientSideRules()) yield break;

//            var formatter = new MessageFormatter().AppendPropertyName(Rule.PropertyName);
//            string message = formatter.BuildMessage(UserResponseMessagesResource.password_weak);
//            var rule = new ModelClientValidationRule
//            {
//                ValidationType = "passwordmeter",
//                ErrorMessage = message
//            };

//            yield return rule;
//        }
//    }
//}
//using FluentValidation.Internal;
//using FluentValidation.Mvc;
//using FluentValidation.Validators;
//using infrastructure.properties.user;
//using System.Collections.Generic;
//using System.Web.Mvc;

//namespace ui.web.Infrastructure.FluentValidators
//{
//    public class StringNoSpacesPropertyValidator : FluentValidationPropertyValidator
//    {
//        public StringNoSpacesPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
//            : base(metadata, controllerContext, rule, validator)
//        {
//        }

//        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
//        {
//            if (!ShouldGenerateClientSideRules()) yield break;

//            var formatter = new MessageFormatter().AppendPropertyName(Rule.PropertyName);
//            string message = formatter.BuildMessage(UserResponseMessagesResource.username_has_spaces);
//            var rule = new ModelClientValidationRule
//            {
//                ValidationType = "nospaces",
//                ErrorMessage = message
//            };

//            yield return rule;
//        }
//    }
//}
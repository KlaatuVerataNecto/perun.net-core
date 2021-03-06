﻿//using FluentValidation.Internal;
//using FluentValidation.Validators;
//using infrastructure.i18n.user;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System.Collections.Generic;

//namespace ui.web.Infrastructure.FluentValidators
//{
//    public class UniqueEmailPropertyValidator : FluentValidationPropertyValidator
//    {
//        public UniqueEmailPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
//            : base(metadata, controllerContext, rule, validator)
//        {
//        }

//        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
//        {
//            if (!this.ShouldGenerateClientSideRules())
//                yield break;

//            var formatter = new MessageFormatter().AppendPropertyName(Rule.PropertyName);
//            string message = formatter.BuildMessage(UserResponseMessagesResource.email_already_used);

//            var rule = new ModelClientValidationRule
//            {
//                ValidationType = "remote",
//                ErrorMessage = message
//            };
//            rule.ValidationParameters.Add("url", "/user/checkemail");
//            rule.ValidationParameters.Add("type", "post");
//            rule.ValidationParameters.Add("minlength", "3");

//            yield return rule;
//        }

//    }
//}
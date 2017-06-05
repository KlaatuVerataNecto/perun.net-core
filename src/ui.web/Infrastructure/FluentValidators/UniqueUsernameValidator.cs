using FluentValidation.Validators;
using infrastructure.i18n.user;
using infrastructure.user.services;

namespace ui.web.Infrastructure.FluentValidators
{
    public class UniqueUsernameValidator : PropertyValidator
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public UniqueUsernameValidator(IUserRegistrationService userRegistrationService)
            : base(UserResponseMessagesResource.username_not_available)
        {
            this._userRegistrationService = userRegistrationService;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string value = context.PropertyValue as string;
            return _userRegistrationService.is_username_available(value, 0);
        }
    }
}
using FluentValidation.Validators;
using infrastructure.i18n.user;
using infrastructure.user.services;
namespace ui.web.Infrastructure.FluentValidators
{
    public class UniqueEmailValidator : PropertyValidator
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public UniqueEmailValidator(IUserRegistrationService userRegistrationService)
            : base(UserResponseMessagesResource.email_already_used)
        {
            this._userRegistrationService = userRegistrationService;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string value = context.PropertyValue as string;
            return _userRegistrationService.is_email_available(value, 0);
        }
    }
}
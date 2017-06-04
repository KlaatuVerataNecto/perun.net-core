using FluentValidation;
using infrastructure.i18n.user;
using infrastructure.user.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ui.web.Infrastructure.FluentValidators;

namespace ui.web.Models.User
{
    public class SignUpModel
    {
        public string username { get; set; }
        private string _email;
        public string email
        {
            get
            {
                if (!string.IsNullOrEmpty(_email))
                    return _email.ToLower();
                else
                    return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string password { get; set; }
        public string password_confirm { get; set; }
        public string return_url { get; set; }
    }

    public class UserCreateModelValidator : AbstractValidator<SignUpModel>
    {
        public UserCreateModelValidator(IUserRegistrationService _userRegistrationService)
        {
            RuleFor(x => x.username).NotEmpty().WithMessage(UserResponseMessagesResource.username_empty);
            RuleFor(x => x.username).SetValidator(new StringNoSpacesValidator());
            RuleFor(x => x.username).Length(3, 50).WithMessage(UserResponseMessagesResource.username_not_in_range);
            RuleFor(x => x.username).SetValidator(new UniqueUsernameValidator(_userRegistrationService));

            RuleFor(x => x.email).NotEmpty().WithMessage(UserResponseMessagesResource.email_empty);
            RuleFor(x => x.email).EmailAddress().WithMessage(UserResponseMessagesResource.email_invalid);
            RuleFor(x => x.email).Length(0, 250).WithMessage(UserResponseMessagesResource.email_not_in_range);
            RuleFor(x => x.email).SetValidator(new UniqueEmailValidator(_userRegistrationService));

            RuleFor(x => x.password).NotEmpty().WithMessage(UserResponseMessagesResource.password_empty);
            RuleFor(x => x.password).Length(6, 60).WithMessage(UserResponseMessagesResource.password_not_in_range);
            RuleFor(x => x.password).SetValidator(new PasswordStrengthValidator());

            RuleFor(x => x.password_confirm).NotEmpty().WithMessage(UserResponseMessagesResource.repeat_new_password_empty);
            RuleFor(x => x.password_confirm).Equal(x => x.password).WithMessage(UserResponseMessagesResource.repeat_new_password_doesnt_match); ;

        }
    }
}

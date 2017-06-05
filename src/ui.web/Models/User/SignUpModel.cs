using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ui.web.Infrastructure.ModelValidators;

namespace ui.web.Models.User
{
    public class SignUpModel
    {
        [Required(ErrorMessageResourceType = typeof(UserResponseMessagesResource),ErrorMessageResourceName = "username_empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "username_not_in_range")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "username_not_in_range")]
        [Remote(action: "VerifyUsername", controller: "Validation")]
        [Display(Name = "Username")]
        public string username { get; set; }
        private string _email;

        [Required(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "email_empty")]
        [EmailAddress(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "email_invalid")]
        [StringLength(250, MinimumLength = 0, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "email_not_in_range")]
        [Remote(action: "VerifyEmail", controller: "Validation")]
        [Display(Name = "Email")]
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

        [Required(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "password_empty")]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "password_not_in_range")]
        [PasswordStrengthValidator(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "password_weak")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "password_nospaces")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "repeat_new_password_empty")]
        [Compare("password", ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "repeat_new_password_doesnt_match")]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password")]
        public string password_confirm { get; set; }
        public string return_url { get; set; }
    }




    // TODO: Whenever Fluent Validation is ready : baaaaam!
    //public class UserCreateModelValidator : AbstractValidator<SignUpModel>
    //{
    //    public UserCreateModelValidator(IUserRegistrationService _userRegistrationService)
    //    {
    //        RuleFor(x => x.username).NotEmpty().WithMessage(UserResponseMessagesResource.username_empty);
    //        RuleFor(x => x.username).SetValidator(new StringNoSpacesValidator());
    //        RuleFor(x => x.username).Length(3, 50).WithMessage(UserResponseMessagesResource.username_not_in_range);
    //        RuleFor(x => x.username).SetValidator(new UniqueUsernameValidator(_userRegistrationService));

    //        RuleFor(x => x.email).NotEmpty().WithMessage(UserResponseMessagesResource.email_empty);
    //        RuleFor(x => x.email).EmailAddress().WithMessage(UserResponseMessagesResource.email_invalid);
    //        RuleFor(x => x.email).Length(0, 250).WithMessage(UserResponseMessagesResource.email_not_in_range);
    //        RuleFor(x => x.email).SetValidator(new UniqueEmailValidator(_userRegistrationService));

    //        RuleFor(x => x.password).NotEmpty().WithMessage(UserResponseMessagesResource.password_empty);
    //        RuleFor(x => x.password).Length(6, 60).WithMessage(UserResponseMessagesResource.password_not_in_range);
    //        RuleFor(x => x.password).SetValidator(new PasswordStrengthValidator());

    //        RuleFor(x => x.password_confirm).NotEmpty().WithMessage(UserResponseMessagesResource.repeat_new_password_empty);
    //        RuleFor(x => x.password_confirm).Equal(x => x.password).WithMessage(UserResponseMessagesResource.repeat_new_password_doesnt_match); ;

    //    }
    //}
}

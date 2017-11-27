using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using peruncore.Infrastructure.ModelValidators;

namespace peruncore.Models.User
{
    public class SignUpModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg),ErrorMessageResourceName = "username_empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_not_in_range")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_has_spaces")]
        [OnlyLettersDigitsUnderScoreValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_has_invalid_characters")]
        [Remote(action: "VerifyUsernameAvailability", controller: "Validation")]
        [Display(Name = "Username")]
        public string username { get; set; }
        private string _email;

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_empty")]
        [EmailAddress(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_invalid")]
        [StringLength(250, MinimumLength = 0, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_not_in_range")]
        [Remote(action: "VerifyEmailAvailability", controller: "Validation")]
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

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_empty")]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_not_in_range")]
        [PasswordStrengthValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_weak")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_nospaces")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "repeat_new_password_empty")]
        [Compare("password", ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "repeat_new_password_doesnt_match")]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password")]
        public string password_confirm { get; set; }
        public string return_url { get; set; }
    }
}

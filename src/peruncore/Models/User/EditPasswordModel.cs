using infrastructure.i18n.user;
using peruncore.Infrastructure.ModelValidators;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class EditPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_empty")]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_not_in_range")]
        [PasswordStrengthValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_weak")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_nospaces")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string password { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "repeat_new_password_empty")]
        [Compare("password", ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "repeat_new_password_doesnt_match")]
        [DataType(DataType.Password)]
        [Display(Name = "Verify new password")]
        public string password_confirm { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_empty")]
        [StringLength(60, MinimumLength = 6, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_not_in_range")]
        [DataType(DataType.Password)]
        [Display(Name = "Your password")]
        public string current_password { get; set; }
    }
}

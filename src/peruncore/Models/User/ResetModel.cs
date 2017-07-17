
using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using peruncore.Infrastructure.ModelValidators;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class ResetModel
    {
        [Required(ErrorMessage = "Userid is required.")]
        public int userid { get; set; }

        [Required(ErrorMessage = "Token is required.")]
        public string token { get; set; }

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
    }
}

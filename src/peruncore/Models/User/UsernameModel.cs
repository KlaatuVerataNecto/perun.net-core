using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using peruncore.Infrastructure.ModelValidators;

namespace peruncore.Models.User
{
    public class UsernameModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg),ErrorMessageResourceName = "username_empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_not_in_range")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_has_spaces")]
        [OnlyLettersDigitsUnderScoreValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_has_invalid_characters")]
        [Remote(action: "VerifyUsernameAvailability", controller: "Validation")]
        [Display(Name = "Username")]
        public string username { get; set; }
        public string token { get; set; }
        public int userid { get; set; }
    }
}

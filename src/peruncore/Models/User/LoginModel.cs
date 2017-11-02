using infrastructure.i18n.user;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_empty")]
        [EmailAddress(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_invalid")]
        [StringLength(250, MinimumLength = 0, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_not_in_range")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "password_empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        public string return_url { get; set; }
    }
}

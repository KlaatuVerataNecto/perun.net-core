using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class EditEmailModel
    {
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
        [DataType(DataType.Password)]
        [Display(Name = "Your Password")]
        public string confirm_password { get; set; }

        public string newemail { get; set; }
    }
}

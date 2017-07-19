using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class ForgotModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_empty")]
        [EmailAddress(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_invalid")]
        [StringLength(250, MinimumLength = 0, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "email_not_in_range")]
        [Remote(action: "VerifyEmailExistance", controller: "Validation")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }
    }
}

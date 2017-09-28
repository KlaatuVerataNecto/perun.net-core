using infrastructure.i18n.user;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using peruncore.Infrastructure.ModelValidators;

namespace peruncore.Models.User
{
    public class ProfileModel
    {
        [Required(ErrorMessageResourceType = typeof(UserValidationMsg),ErrorMessageResourceName = "username_empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_not_in_range")]
        [StringNoSpacesValidator(ErrorMessageResourceType = typeof(UserValidationMsg), ErrorMessageResourceName = "username_not_in_range")]
        [Remote(action: "VerifyUsername", controller: "Validation")]
        [Display(Name = "Username")]
        public string username { get; set; }    
    }
}

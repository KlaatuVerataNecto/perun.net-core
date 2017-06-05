using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ui.web.Infrastructure.ModelValidators;

namespace ui.web.Models.User
{
    public class LoginModel
    {
        // TODO: Move error messages to Resource file
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Remote("isEmailAvailable", "Validation")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringNoSpacesValidator]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        public string return_url { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class LoginModel
    {
        // TODO: Move error messages to Resource file
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        public string return_url { get; set; }
    }
}

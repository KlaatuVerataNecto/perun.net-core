using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class ForgotModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }
    }
}

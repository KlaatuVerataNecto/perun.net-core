using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace peruncore.Models.User
{
    public class UserCoverModel
    {
        [Required]
        public IFormFile cover_image { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.Common
{
    public class ImageModel
    {
        [Required]
        public IFormFile file { get; set; }
    }
}

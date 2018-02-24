using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using infrastructure.i18n.post;

namespace peruncore.Models.Post
{
    public class PostModel
    {
        [Required(ErrorMessageResourceType = typeof(PostValidationMsg), ErrorMessageResourceName = "post_title_empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(PostValidationMsg), ErrorMessageResourceName = "post_title_not_in_range")]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required(ErrorMessageResourceType = typeof(PostValidationMsg), ErrorMessageResourceName = "post_image_empty")]
        [StringLength(40, MinimumLength = 40, ErrorMessageResourceType = typeof(PostValidationMsg), ErrorMessageResourceName = "post_image_not_in_range")]
        public string post_image { get; set; }
    }
}

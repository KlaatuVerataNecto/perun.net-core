using infrastructure.i18n.user;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace peruncore.Models.User
{
    public class UserAvatarModel
    {
        public IFormFile avatar_image { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "crop_area_invalid")]
        public int avatar_x { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "crop_area_invalid")]
        public int avatar_y { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "crop_area_invalid")]
        public int avatar_width { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(UserResponseMessagesResource), ErrorMessageResourceName = "crop_area_invalid")]
        public int avatar_height { get; set; }
    }
}


namespace peruncore.Config
{
    public class ImageUploadSettings
    {
        public string AvatarImagePath { get; set; }
        public string UploadPath { get; set; }        
        public string DefaultImageExtension { get; set; }
        public int UserAvatarQuality { get; set; }
        public int UserAvatarWidth { get; set; }
        public int UserAvatarHeight { get; set; }
        public string AvatarImageDirURL { get; set; }
        public string ImageDefaultDirURL { get; set; }
    }
}

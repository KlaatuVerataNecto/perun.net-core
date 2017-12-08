
namespace peruncore.Config
{
    public class ImageUploadSettings
    {
        public string AvatarImageUploadPath { get; set; }
        public string AvatarImagePath { get; set; }
        public string AvatarImageDirURL { get; set; }
        public int AvatarImageQuality { get; set; }
        public int AvatarImageWidth { get; set; }
        public int AvatarImageHeight { get; set; }


        public string PostImageUploadPath { get; set; }
        public string PostImagePath { get; set; }
        public string PostImageDirURL { get; set; }
        public int PostImageQuality { get; set; }
        public int PostImageMaxWidth { get; set; }
        public int PostImageMaxHeight { get; set; }
        public string ImageDefaultDirURL { get; set; }
        public string DefaultImageExtension { get; set; }
    }
}
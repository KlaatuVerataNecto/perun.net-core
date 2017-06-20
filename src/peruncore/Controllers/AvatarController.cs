using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

using infrastucture.libs.image;
using peruncore.Config;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class AvatarController : Controller
    {
        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly ILogger _logger;
        public AvatarController(IOptions<ImageUploadSettings> imageUploadSettings, ILogger<AvatarController> logger)
        {
            _imageUploadSettings = imageUploadSettings.Value;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult upload(UserAvatarModel model)
        {
            string filePathUploaded = _imageUploadSettings.UploadPath + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + _imageUploadSettings.DefaultImageExtension;
            string filePathResized = _imageUploadSettings.AvatarImagePath + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + _imageUploadSettings.DefaultImageExtension;

            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
            {
                model.avatar_image.CopyTo(stream);
            }

            var config = new ImageConfig()
                                .CreateConfigFromImageFile(filePathUploaded)
                                .WithSaveTo(filePathResized);

            var image = ImageService.ResizeAndSave(config);

            // TODO: That's temporary
            var absUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            var image_url = absUrl+ "/images/" + Path.GetFileName(config.SavePathFile);
            return Redirect(image_url);
        }
    }
}
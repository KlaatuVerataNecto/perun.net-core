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
            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.avatar_image.CopyToAsync(stream);
            }

            string filePathToSave = _imageUploadSettings.AvatarImagePath + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + _imageUploadSettings.DefaultImageExtension;
            var config = new ImageConfig()
                            .CreateConfigFromImageFile(filePath)
                            .WithSaveTo(filePathToSave);

            _logger.LogInformation("Temporary image to upload:" + filePath);

            var image = ImageService.ResizeAndSave(config);

            // TODO: That's temporary
            var absUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            var image_url = absUrl+ "/images/" + Path.GetFileName(config.SavePathFile);
            return Redirect(image_url);
        }
    }
}
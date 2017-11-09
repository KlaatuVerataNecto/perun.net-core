#region # using statements #

using System;
using System.IO;
using infrastucture.libs.image;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using peruncore.Config;
using peruncore.Models.User;

#endregion

namespace peruncore.Controllers
{
    public class AvatarController : Controller
    {

        #region # Variables #

        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly IImageService _imageService;
        private readonly ILogger _logger;

        #endregion

        public AvatarController(IOptions<ImageUploadSettings> imageUploadSettings,
            IImageService imageService, ILogger<AvatarController> logger)
        {
            _imageUploadSettings = imageUploadSettings.Value;
            _imageService = imageService;
            _logger = logger;
        }

        #region # Methods #

        #region == Actions ==

        [HttpPost]
        public ActionResult Upload(UserAvatarModel model)
        {
            var filePathUploaded = Path.Combine(_imageUploadSettings.UploadPath,
                Guid.NewGuid() + _imageUploadSettings.DefaultImageExtension);
            var filePathResized = Path.Combine(_imageUploadSettings.AvatarImagePath,
                Guid.NewGuid() + _imageUploadSettings.DefaultImageExtension);

            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
            {
                model.avatar_image.CopyTo(stream);

                // Resize image
                var config = new ImageConfigBuilder()
                    .WithSourceFilePath(filePathUploaded)
                    .WithSaveFilePath(filePathResized)
                    .WithQuality(85)
                    .WithWidth(_imageUploadSettings.UserAvatarWidth)
                    .WithHeight(_imageUploadSettings.UserAvatarHeight)
                    .Build();

                _imageService.Resize(config);
            }

            // TODO: Persist the avatar path to the user database

            // Done
            var absUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            var imageUrl = absUrl + "/images/" + Path.GetFileName(filePathResized);

            return Json(new {imageUrl});
        }

        #endregion

        #endregion

    }
}
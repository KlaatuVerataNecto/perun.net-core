#region # using statements #

using System;
using System.IO;
using System.Security.Claims;
using infrastructure.user.interfaces;
using infrastucture.libs.image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using peruncore.Models.Common;

#endregion

namespace peruncore.Controllers
{
    public class ImageController : Controller
    {

        #region # Variables #

        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IImageService _imageService;
        private readonly IUserAccountService _userAccountService;
        private readonly ILogger _logger;

        #endregion

        public ImageController(
            IOptions<ImageUploadSettings> imageUploadSettings,
            IOptions<AuthSchemeSettings> authSchemeSettings,
            IHostingEnvironment hostingEnvironment, 
            IImageService imageService,
            IUserAccountService userAccountService, 
            ILogger<ImageController> logger)
        {
            _imageUploadSettings = imageUploadSettings.Value;
            _authSchemeSettings = authSchemeSettings.Value;
            _hostingEnvironment = hostingEnvironment;
            _imageService = imageService;
            _userAccountService = userAccountService;
            _logger = logger;
        }

        #region # Methods #

        #region == Actions ==

        [HttpPost]
        [Authorize]
        public ActionResult Cover(ImageModel model)
        {
            // TODO: DRY see PostController
            // TODO: Validate
            if (!TryValidateModel(model))
                return BadRequest();

            var ext = _imageService.getImageExtensionByContentType(model.file.ContentType);
            var filePathUploaded = Path.Combine(_hostingEnvironment.ContentRootPath,
                _imageUploadSettings.CoverImagePath,
                                    Guid.NewGuid() + ext);

            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
                model.file.CopyTo(stream);

            var imageFilename = Path.GetFileName(filePathUploaded);

            var identity = (ClaimsIdentity)User.Identity;
            var userUsername = _userAccountService.getUsernameByUserId(identity.GetUserId());
            var avatarChange = _userAccountService.changeCover(userUsername.UserId, imageFilename);

            return Json(new { imageUrl = _imageUploadSettings.CoverImageDirURL + imageFilename });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Avatar(ImageModel model)
        {
            // TODO: DRY see PostController
            // TODO: Validate
            model = model ?? new ImageModel();
            if (!TryValidateModel(model))
                return BadRequest();

            var ext = _imageService.getImageExtensionByContentType(model.file.ContentType);
            var filePathUploaded = Path.Combine(_hostingEnvironment.ContentRootPath,
                _imageUploadSettings.AvatarImageUploadPath,
                                                Guid.NewGuid() + ext);
            
            var filePathResized = Path.Combine(_hostingEnvironment.ContentRootPath,
                _imageUploadSettings.AvatarImagePath,
                Guid.NewGuid() + _imageUploadSettings.DefaultImageExtension);

            // Copy the file
            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
                model.file.CopyTo(stream);

            // Crop the image
            var config = new ImageConfigBuilder()
                         .WithSourceFilePath(filePathUploaded)
                         .WithSaveFilePath(filePathResized)
                .WithMaxWidth(_imageUploadSettings.AvatarImageWidth)
                         .WithQuality(_imageUploadSettings.AvatarImageQuality)                         
                         .Build();

            _imageService.Resize(config);
            
            //
            var identity = (ClaimsIdentity)User.Identity;
            var imageFilename = Path.GetFileName(filePathResized);

            var userUsername = _userAccountService.getUsernameByUserId(identity.GetUserId());
            var avatarChange =_userAccountService.changeAvatar(userUsername.UserId, imageFilename);

            if (avatarChange == null)
                return BadRequest("Unable to change avatar");

            // TODO: DRY
            HttpContext.SignInAsync(
                _authSchemeSettings.Application,
                ClaimsPrincipalFactory.Build(
                    identity.GetUserId(),
                    identity.GetLoginId(),
                    identity.GetUserName(),
                    identity.GetUserName(),
                    identity.GetRoles(),
                    imageFilename,
                    identity.GetProvider()),
                new AuthenticationProperties { IsPersistent = true }
            );

            // Done
            return Json(new {imageUrl = _imageUploadSettings.AvatarImageDirURL + imageFilename});
        }

        public IActionResult Post(ImageModel model)
        {
            // TODO: DRY 
            var ext = Path.GetExtension(model.file.FileName);

            var filePathUploaded = Path.Combine(
                _hostingEnvironment.ContentRootPath,
                _imageUploadSettings.PostImageUploadPath,
                Guid.NewGuid() + (string.IsNullOrWhiteSpace(ext) ? "jpeg" : ext)
            );

            var filePathResized = Path.Combine(
                _hostingEnvironment.ContentRootPath,
                _imageUploadSettings.PostImagePath,
                Guid.NewGuid() + _imageUploadSettings.DefaultImageExtension
            );

            // Copy the file
            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
                model.file.CopyTo(stream);

            // Crop the image
            var config = new ImageConfigBuilder()
                         .WithSourceFilePath(filePathUploaded)
                         .WithSaveFilePath(filePathResized)
                         .WithQuality(_imageUploadSettings.PostImageQuality)
                         .WithMaxWidth(_imageUploadSettings.PostImageMaxWidth)
                         .WithMaxHeight(_imageUploadSettings.PostImageMaxHeight)
                         .Build();

            _imageService.Resize(config);

            var imageFilename = Path.GetFileName(filePathResized);

            // Done
            return Json(new { imageUrl = _imageUploadSettings.PostImageDirURL + imageFilename, imageFile = imageFilename });
        }

        #endregion

        #endregion

    }
}
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

#endregion

namespace peruncore.Controllers
{
    public class AvatarController : Controller
    {

        #region # Variables #

        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IImageService _imageService;
        private readonly IUserAccountService _userAccountService;
        private readonly ILogger _logger;

        #endregion

        public AvatarController(IOptions<ImageUploadSettings> imageUploadSettings,
            IOptions<AuthSchemeSettings> authSchemeSettings,
            IHostingEnvironment hostingEnvironment, IImageService imageService,
            IUserAccountService userAccountService, ILogger<AvatarController> logger)
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
        public ActionResult Upload(UserAvatarModel model)
        {
            model = model ?? new UserAvatarModel();
            if (!TryValidateModel(model))
                return BadRequest();

            var ext = Path.GetExtension(model.avatar_image.FileName);
            var filePathUploaded = Path.Combine(_hostingEnvironment.ContentRootPath,
                _imageUploadSettings.UploadPath,
                Guid.NewGuid() + (string.IsNullOrWhiteSpace(ext) ? "jpeg" : ext));
            var filePathResized = Path.Combine(_hostingEnvironment.ContentRootPath,
                _imageUploadSettings.AvatarImagePath,
                Guid.NewGuid() + _imageUploadSettings.DefaultImageExtension);

            using (var stream = new FileStream(filePathUploaded, FileMode.Create))
            {
                model.avatar_image.CopyTo(stream);

                // Resize image
                var config = new ImageConfigBuilder()
                             .WithSourceFilePath(filePathUploaded)
                             .WithSaveFilePath(filePathResized)
                             .WithQuality(_imageUploadSettings.UserAvatarQuality)
                             .WithX(model.avatar_x)
                             .WithY(model.avatar_y)
                             .WithWidth(model.avatar_width)
                             .WithHeight(model.avatar_height)
                             .Build();

                _imageService.Crop(config);
            }
            
            //
            var identity = (ClaimsIdentity)User.Identity;
            var absUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            var imageUrl = absUrl + "/images/useravatar/" +
                           Path.GetFileName(filePathResized);

            var userUsername =
                _userAccountService.getUsernameByUserId(identity.GetUserId());
            var avatarChange =
                _userAccountService.changeAvatar(userUsername.UserId, imageUrl);

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
                    imageUrl,
                    identity.GetProvider()),
                new AuthenticationProperties { IsPersistent = true }
            );

            // Done
            return Json(new {imageUrl});
        }

        #endregion

        #endregion

    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using infrastructure.user.interfaces;

namespace peruncore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly ImageUploadSettings _imageUploadSettings;

        public UserController(
            IUserAccountService userAccountService,
            IOptions<ImageUploadSettings> imageUploadSettings,
            IOptions<AuthSchemeSettings> authSchemeSettings
            )
        {
            _userAccountService = userAccountService;
            _authSchemeSettings = authSchemeSettings.Value;
            _imageUploadSettings = imageUploadSettings.Value;
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(_authSchemeSettings.Application);
            HttpContext.Session.Remove("LoginId");
            return Redirect("/");
        }    


        [Route("user/{id:int}/{username}")]
        public IActionResult Index(int id, string username)
        {
            var userProfile = _userAccountService.getUserProfile(id);

            if(userProfile == null)
            {
                return RedirectToAction("Index", "Error");
            }

            bool canChange = User.Identity.IsAuthenticated && ((ClaimsIdentity)User.Identity).GetUserId() == userProfile.UserId;

            var model = new ProfileViewModel(
                    userProfile.Username,
                    string.Empty,
                    userProfile.Avatar,
                _imageUploadSettings.AvatarImageDirURL,
                _imageUploadSettings.ImageDefaultDirURL,
                canChange
            );

            return View(model);
        }
    }
}
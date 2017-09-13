using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using infrastructure.user.interfaces;
using peruncore.Infrastructure.Auth;
using peruncore.Config;
using Microsoft.Extensions.Options;

namespace peruncore.Controllers
{
    public class ValidationController : Controller
    {
        private IUserRepository _userRepository;
        private readonly AuthSchemeSettings _authSchemeSettings;

        public ValidationController(IUserRepository userRepository, IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _userRepository = userRepository;
            _authSchemeSettings = authSchemeSettings.Value;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(string email)
       {
            var identity = (ClaimsIdentity)User.Identity;

            if (identity != null && identity.GetProvider() == _authSchemeSettings.Application && email == identity.GetEmail())
            {
                return Json(data: true);
            }

            if (_userRepository.isEmailAvailable(email))
            {
                return Json(data: true);
            }

            // TODO: i18n
            return Json(data: $"Email {email} is already in use.");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyUsername(string username)
        {
            var identity = (ClaimsIdentity)User.Identity;

            if (identity != null && identity.GetProvider() == _authSchemeSettings.Application && username == identity.GetUserName())
            {
                return Json(data: true);
            }

            if (_userRepository.isUsernameAvailable(username))
            {
                return Json(data: true);
            }

            // TODO: i18n
            return Json(data: $"Username {username} is already in use.");
        }
    }
}
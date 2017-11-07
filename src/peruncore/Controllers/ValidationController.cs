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
        public IActionResult VerifyEmailExistance(string email)
        {
            if (!_userRepository.isEmailAvailable(email))
                return Json(data: true);
            else
                return Json(data: $"Email {email} is not registered.");
       }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmailAvailability(string email)
       {
            var identity = (ClaimsIdentity)User.Identity;

            if (identity != null && 
                identity.GetProvider() == _authSchemeSettings.Application && 
                email.ToLower() == identity.GetEmail()
                )
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
        public IActionResult VerifyUsernameAvailability(string username)
        {
            var identity = (ClaimsIdentity)User.Identity;

            if (identity != null  && username == identity.GetUserName())
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
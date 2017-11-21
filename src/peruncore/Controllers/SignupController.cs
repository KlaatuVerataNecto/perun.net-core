using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

using infrastructure.user.interfaces;
using infrastructure.i18n.user;
using peruncore.Models.User;
using peruncore.Config;
using peruncore.Infrastructure.Auth;


namespace peruncore.Controllers
{
    public class SignupController : Controller
    {
        private IUserRegistrationService _userRegistrationService;
        private readonly AuthSettings _authSettings;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private const string SessionKeyName = "_RequestUserNamkeToken";

        public SignupController(IUserRegistrationService userRegistrationService, 
                                IOptions<AuthSettings> authSettings, 
                                IOptions<AuthSchemeSettings> authSchemeSettings
        )
        {
            _userRegistrationService = userRegistrationService;
            _authSettings = authSettings.Value;
            _authSchemeSettings = authSchemeSettings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SignUpModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var userIdentity = _userRegistrationService.signup(
                model.username,
                model.email, 
                model.password, 
                _authSchemeSettings.Application,
                _authSettings.SaltLength);

            if (userIdentity == null)
            {
                ModelState.AddModelError("email", UserValidationMsg.email_password_incorrect);
                return View("Index", model);
            }

            // TODO: Duplicated code (see login and oauth)
            HttpContext.SignInAsync(
                _authSchemeSettings.Application,
                ClaimsPrincipalFactory.Build(
                    userIdentity.UserId,
                    userIdentity.LoginId,
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar,
                    userIdentity.LoginProvider)
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
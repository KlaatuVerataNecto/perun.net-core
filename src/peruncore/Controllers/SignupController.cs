using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using peruncore.Models.User;
using infrastructure.user.services;
using peruncore.Config;
using Microsoft.Extensions.Options;
using peruncore.Infrastructure.Auth;

namespace peruncore.Controllers
{
    public class SignupController : Controller
    {
        private IUserRegistrationService _userRegistrationService;
        private readonly AuthSettings _authSettings;
        private readonly AuthSchemeSettings _authSchemeSettings;

        public SignupController(IUserRegistrationService userRegistrationService, IOptions<AuthSettings> authSettings, IOptions<AuthSchemeSettings> authSchemeSettings)
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

            var userIdentity = _userRegistrationService.Signup(model.username,model.email, model.password, _authSchemeSettings.Default, _authSettings.SaltLength);

            if (userIdentity == null)
            {
                // TODO: Get error message for resource file 
                ModelState.AddModelError("email", "Incorrect email or password.");
                return View("Index", model);
            }

            HttpContext.Authentication.SignInAsync(_authSchemeSettings.Default,
                ClaimsPrincipalFactory.Build(
                    userIdentity.UserId,
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar)
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
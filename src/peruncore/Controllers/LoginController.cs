using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using infrastructure.user.services;
using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class LoginController : Controller
    {
        private IUserAuthentiactionService _userAuthentiactionService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        public LoginController(IUserAuthentiactionService userAuthentiactionService, IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _userAuthentiactionService = userAuthentiactionService;
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
        public IActionResult Index(LoginModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);
                       
            var userIdentity = _userAuthentiactionService.login(model.email, model.password);

            if(userIdentity == null )
            {
                // TODO: Get error message for resource file 
                ModelState.AddModelError("email","Incorrect email or password.");
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
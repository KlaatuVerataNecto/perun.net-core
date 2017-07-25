using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Http.Authentication;

namespace peruncore.Controllers
{
    public class LoginController : Controller
    {
        private IUserAuthentiactionService _userAuthentiactionService;
        private readonly AuthSchemeSettings _authSchemeSettings;

        public LoginController(
            IUserAuthentiactionService userAuthentiactionService, 
            IOptions<AuthSchemeSettings> authSchemeSettings
            )
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

            // TODO: Duplicated code
            HttpContext.Authentication.SignInAsync(
                _authSchemeSettings.Application, 
                ClaimsPrincipalFactory.Build(
                    userIdentity.UserId, 
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    }
                );
            return RedirectToAction("Index", "Home");
        }
    }
}
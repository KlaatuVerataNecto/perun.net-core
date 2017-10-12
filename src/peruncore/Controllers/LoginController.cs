using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using peruncore.Config;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Http.Authentication;
using infrastructure.i18n.user;

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
                ModelState.AddModelError("email", UserValidationMsg.email_password_incorrect);
                return View("Index", model);
            }

            // TODO: Duplicated code
            HttpContext.Authentication.SignInAsync(
                _authSchemeSettings.Application, 
                ClaimsPrincipalFactory.Build(
                    userIdentity.UserId,
                    userIdentity.LoginId,
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar,
                    userIdentity.LoginProvider
                    ),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    }
                );
            return RedirectToAction("Index", "Home");
        }
    }
}
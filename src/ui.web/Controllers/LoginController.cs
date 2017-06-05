using infrastructure.user.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ui.web.Config;
using ui.web.Infrastructure.Auth;
using ui.web.Models.User;

namespace ui.web.Controllers
{
    public class LoginController : Controller
    {
        private IUserAuthentiactionService _userAuthentiactionService;

        public LoginController(IUserAuthentiactionService userAuthentiactionService)
        {
            _userAuthentiactionService = userAuthentiactionService;
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

            HttpContext.Authentication.SignInAsync(ConfigVariables.AuthSchemeName, 
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
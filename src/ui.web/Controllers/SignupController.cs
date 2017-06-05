using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ui.web.Models.User;
using infrastructure.user.services;

namespace ui.web.Controllers
{
    public class SignupController : Controller
    {
        private IUserRegistrationService _userRegistrationService;

        public SignupController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
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

            //var userIdentity = _userRegistrationService.login(model.email, model.password);

            //if (userIdentity == null)
            //{
            //    // TODO: Get error message for resource file 
            //    ModelState.AddModelError("email", "Incorrect email or password.");
            //    return View("Index", model);
            //}

            //HttpContext.Authentication.SignInAsync(ConfigVariables.AuthSchemeName,
            //    ClaimsPrincipalFactory.Build(
            //        userIdentity.UserId,
            //        userIdentity.Username,
            //        userIdentity.Email,
            //        userIdentity.Roles,
            //        userIdentity.Avatar)
            //    );
            return RedirectToAction("Index", "Home");
        }
    }
}
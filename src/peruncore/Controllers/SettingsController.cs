using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication;

using infrastructure.user.interfaces;
using infrastructure.i18n.user;
using infrastructure.email.interfaces;
using peruncore.Infrastructure.Extensions;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using peruncore.Config;


namespace peruncore.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private IEmailService _emailService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly AuthSettings _authSettings;
        private readonly ILogger _logger;

        public SettingsController(
            IUserAccountService userAccountService,
            IEmailService emailService,
            IOptions<AuthSchemeSettings> authSchemeSettings, 
            IOptions<AuthSettings> authSettings,
            ILogger<AvatarController> logger
            )
        {
            _authSettings = authSettings.Value;
            _userAccountService = userAccountService;
            _authSchemeSettings = authSchemeSettings.Value;
            _emailService = emailService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Account()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return View(new AccountModel() {  username = identity.GetUserName()});
        }

        [Authorize]
        [HttpPost]
        public IActionResult Account(AccountModel model)
        {
            if (!ModelState.IsValid) return View("Account", model);

            var identity = (ClaimsIdentity)User.Identity;

            var userUsername = _userAccountService.getUsernameByUserId(identity.GetUserId());

            if (userUsername.Username == model.username)
            {
                ModelState.AddModelError("username", UserValidationMsg.username_not_modified);
                return View("Account", model);
            }

            var usernameChange = _userAccountService.changeUsername(userUsername.UserId,model.username);

            if (usernameChange == null)
            {
                ModelState.AddModelError("username", UserValidationMsg.password_incorrect);
            }

            // TODO: Move to signin manager service 
            HttpContext.SignInAsync(
            _authSchemeSettings.Application,
             ClaimsPrincipalFactory.Build(
                identity.GetUserId(),
                identity.GetLoginId(),
                usernameChange,
                identity.GetUserName(),
                identity.GetRoles(),
                identity.GetAvatar(),
                identity.GetProvider()),
            new AuthenticationProperties { IsPersistent = true }
            );

            TempData["username_change_ok"] = UserValidationMsg.username_change_ok;
            return RedirectToAction("Account", "settings");
        }

        [Authorize]
        public IActionResult Logins()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var list = _userAccountService.getLoginsByUserId(identity.GetUserId());
            return View(new UserLoginsModel(list, _authSchemeSettings));
        }
        [Authorize]
        public IActionResult Email()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var appLogin = _userAccountService.getApplicationLoginById(identity.GetUserId());
            if (appLogin == null)
            {
                _logger.LogInformation("User doesn't have any App Login.", new object[] { identity.GetLoginId(), identity.GetEmail(), identity.GetProvider() });
                return RedirectToAction("index", "error");
            }
            var newEmail = _userAccountService.getPendingNewEmailActivation(identity.GetUserId());
            return View(new EditEmailModel { email = appLogin.Email, newemail = newEmail});        
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeEmail(EditEmailModel model)
        {
            if (!ModelState.IsValid) return View("Email", model);

            var identity = (ClaimsIdentity)User.Identity;

            var appLogin =_userAccountService.getApplicationLoginById(identity.GetUserId());

            if (appLogin.Email == model.email)
            {
                ModelState.AddModelError("email", UserValidationMsg.email_not_modified);
                return View("Email", model);
            }

            var emailChange = _userAccountService.createEmailChangeRequest(
                identity.GetUserId(), 
                model.confirm_password,
                model.email, 
                _authSettings.ResetTokenLength, 
                _authSettings.ExpiryDays
                );

            if (emailChange == null)
            {
                ModelState.AddModelError("confirm_password", UserValidationMsg.password_incorrect);
                return View("Email", model);
            }

            string url = CustomUrlHelperExtensions.AbsoluteAction(
                new UrlHelper(this.ControllerContext),
                "change",
                "email",
                new { id = emailChange.UserId, token = emailChange.EmailToken }
            );

            _emailService.sendEmailChangeActivation(emailChange.EmailTo, url, emailChange.EmailTokenExpiryDate);

            return RedirectToAction("email","settings");
        }

        [Authorize]
        public IActionResult Password()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (_userAccountService.getApplicationLoginById(identity.GetUserId()) == null)
            {
                _logger.LogInformation("User doesn't have any App Login.", new object[] { identity.GetLoginId(), identity.GetEmail(), identity.GetProvider() });
                return RedirectToAction("index", "error");
            }

            return View(new EditPasswordModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(EditPasswordModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (!ModelState.IsValid) return View("Password", model);

            if (_userAccountService.getApplicationLoginById(identity.GetUserId()) == null)
            {
                _logger.LogInformation("User doesn't have any App Login.", new object[] { identity.GetLoginId(), identity.GetEmail(), identity.GetProvider() });
                return RedirectToAction("index", "error");
            }

            var passwordChange = _userAccountService.changePassword(
               identity.GetUserId(),
               model.current_password,
               model.password,
               _authSettings.SaltLength
            );

            if (!passwordChange)
            {
                ModelState.AddModelError("current_password", UserValidationMsg.password_incorrect);
                return View("Password", model);
            }

            TempData["password_change_ok"] = UserValidationMsg.password_change_ok;
            return RedirectToAction("password", "settings");

        }


        // TODO: add [Authorize]
        public IActionResult Avatar()
        {
            return View();
        }
    }
}
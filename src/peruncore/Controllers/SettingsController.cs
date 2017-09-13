using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;
using peruncore.Config;
using Microsoft.Extensions.Options;
using infrastructure.i18n.user;
using peruncore.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using infrastructure.email.interfaces;

namespace peruncore.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private IEmailService _emailService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly AuthSettings _authSettings;

        public SettingsController(
            IUserAccountService userAccountService,
            IEmailService emailService,
            IOptions<AuthSchemeSettings> authSchemeSettings, 
            IOptions<AuthSettings> authSettings
            )
        {
            _authSettings = authSettings.Value;
            _userAccountService = userAccountService;
            _authSchemeSettings = authSchemeSettings.Value;
            _emailService = emailService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logins()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var list = _userAccountService.getLoginsByUserId(identity.GetUserId());
            return View(list);
        }
        [Authorize]
        public IActionResult Email()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (identity.GetProvider() != _authSchemeSettings.Application)
            {
                // TODO: log it
                // User not logged with Application account and tries to change email .... weird.
                return RedirectToAction("index", "error");
            }

            return View(new EditEmailModel { email = identity.GetEmail()});        
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeEmail(EditEmailModel model)
        {
            if (!ModelState.IsValid) return View("Email", model);

            var identity = (ClaimsIdentity)User.Identity;

            if (identity.GetEmail() == model.email)
            {
                ModelState.AddModelError("confirm_password", UserValidationMsg.email_not_modified);
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
                ModelState.AddModelError("new_email", UserValidationMsg.password_incorrect);
                return View("Email", model);
            }

            string url = CustomUrlHelperExtensions.AbsoluteAction(
                new UrlHelper(this.ControllerContext),
                "change",
                "email",
                new { id = emailChange.UserId, token = emailChange.EmailToken }
            );

            // TODO: use email templates 
            _emailService.sendPasswordReminder(emailChange.EmailTo, url, emailChange.EmailTokenExpiryDate);
            return View();
        }

        [Authorize]
        public IActionResult Password()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword()
        {
            return View();
        }


        // TODO: add [Authorize]
        public IActionResult Avatar()
        {
            return View();
        }
    }
}
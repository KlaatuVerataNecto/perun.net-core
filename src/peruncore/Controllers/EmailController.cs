using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

using infrastructure.i18n.user;
using infrastructure.user.interfaces;
using peruncore.Infrastructure.Auth;
using peruncore.Config;


namespace peruncore.Controllers
{
    public class EmailController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly ILogger _logger;

        public EmailController(
            ILogger<AvatarController> logger,
            IUserAccountService userAccountService,
            IOptions<AuthSchemeSettings> authSchemeSettings
            )
        {
            _logger = logger;
            _authSchemeSettings = authSchemeSettings.Value;
            _userAccountService = userAccountService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Change(int id, string token)
        {
            var emailChanged = _userAccountService.applyEmailByToken(id, token);
            if (emailChanged == null)
            {
                _logger.LogInformation("Email change token has expired.", new object[] { id, token });
                return RedirectToAction("EmailChangeTokenExpired", "Error");
            }

            // TODO: Move to signin manager service 
            var identity = (ClaimsIdentity)User.Identity;

            if (identity.GetEmail() == emailChanged.OldEmail)
            {
                // TODO: Move to signin manager service 
                HttpContext.SignInAsync(
                _authSchemeSettings.Application,
                 ClaimsPrincipalFactory.Build(
                    identity.GetUserId(),
                    identity.GetLoginId(),
                    identity.GetUserName(),
                    emailChanged.CurrentEmail,
                    identity.GetRoles(),
                    identity.GetAvatar(),
                    identity.GetProvider()),
                new AuthenticationProperties { IsPersistent = true }
                );
            }
            TempData["email_change_ok"] = UserValidationMsg.email_change_ok;
            return RedirectToAction("email","settings");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Cancel()
        {
            var identity = (ClaimsIdentity)User.Identity;
            _userAccountService.cancelEmailActivation(identity.GetUserId());
            return RedirectToAction("email", "settings");
        }

    }
}
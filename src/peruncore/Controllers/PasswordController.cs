using infrastructure.email.interfaces;
using infrastructure.i18n.user;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using peruncore.Config;
using peruncore.Infrastructure.Extensions;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class PasswordController : Controller
    {
        private IUserPasswordService _userPasswordService;
        private IEmailService _emailService;
        private readonly AuthSettings _authSettings;
        private readonly ILogger _logger;

        public PasswordController(
            IUserPasswordService userPasswordService, 
            IEmailService emailService,
            IOptions<AuthSettings> authSettings,
            ILogger<PasswordController> logger)
        {
            _authSettings = authSettings.Value;
            _userPasswordService = userPasswordService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult BeginForgot(ForgotModel model)
        {
            if (!ModelState.IsValid) return View("Forgot", model);
            var userReset = _userPasswordService.generateResetToken(model.email, _authSettings.ResetTokenLength, _authSettings.ExpiryDays);

            if (userReset == null)
            {
                ModelState.AddModelError("email", UserValidationMsg.password_change_expired);
                return View("Forgot", model);
            }
            string url = CustomUrlHelperExtensions.AbsoluteAction(
                new UrlHelper(this.ControllerContext),
                "reset",
                "password",
                new { id = userReset.UserId, token = userReset.PasswordToken }
            );
            
            _emailService.sendPasswordReminder(userReset.EmailTo, url, userReset.PasswordTokenExpiryDate);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Reset(int id, string token)
        {
            var userReset = _userPasswordService.verifyToken(id,token);
            if(userReset == null)
            {
                _logger.LogInformation("Password Reset Token has expired.", new object[] { id, token});
                return RedirectToAction("ResetTokenExpired","Error");
            }
            return View(new ResetModel {
                userid = userReset.UserId,
                token = userReset.PasswordToken
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult FinishForgot(ResetModel model)
        {
            if (!ModelState.IsValid) return View("Reset", new { id = model.userid, token = model.token});
            var userReset = _userPasswordService.changePassword(model.userid, model.token, model.password);

            if (userReset == null)
            {
                _logger.LogInformation("Unable to reset user's password. User ID or Token doesn't match.", new object[] { model.userid, model.token });
                ModelState.AddModelError("password", UserValidationMsg.error_wierd_shit_going_on);
                return View("Reset", new { id = model.userid, token = model.token });
            }

            return View();
        }
    }
}
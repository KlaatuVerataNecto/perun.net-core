using infrastructure.email.interfaces;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using peruncore.Config;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class PasswordController : Controller
    {
        private IUserPasswordService _userPasswordService;
        private IEmailService _emailService;
        private readonly AuthSettings _authSettings;
        public PasswordController(IUserPasswordService userPasswordService, IEmailService emailService, IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
            _userPasswordService = userPasswordService;
            _emailService = emailService;
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
            var userReset = _userPasswordService.generateResetToken(model.email, _authSettings.ResetTokenLength, _authSettings.ExpiryDays);
            ViewBag.Token = userReset.PasswordToken;
            _emailService.sendPasswordReminder(userReset.EmailTo, userReset.PasswordToken, userReset.PasswordTokenExpiryDate);
            return View();
        }
    }
}
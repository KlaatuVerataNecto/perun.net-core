using infrastructure.email.interfaces;
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
          
            string url = CustomUrlHelperExtensions.AbsoluteAction(
                new UrlHelper(this.ControllerContext),
                "reset",
                "password",
                new { id = userReset.UserId, token = userReset.PasswordToken }
            );
            
            _emailService.sendPasswordReminder(userReset.EmailTo, url, userReset.PasswordTokenExpiryDate);
            ViewBag.Token = url;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Reset(int id, string token)
        {
            var userReset = _userPasswordService.verifyToken(id,token);
            if(userReset == null)
            {
                // TODO: redirect to error, create log entry
            }
            return View();
        }
    }
}
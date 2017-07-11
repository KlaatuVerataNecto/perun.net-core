using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class ValidationController : Controller
    {
        private IUserRepository _userRepository;

        public ValidationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(string email)
        {
            if (_userRepository.isEmailAvailable(email))
            {
                return Json(data: true);
            }

            // TODO: i18n
            return Json(data: $"Email {email} is already in use.");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyUsername(string username)
        {
            if (_userRepository.isUsernameAvailable(username))
            {
                return Json(data: true);
            }

            // TODO: i18n
            return Json(data: $"Username {username} is already in use.");
        }
    }
}
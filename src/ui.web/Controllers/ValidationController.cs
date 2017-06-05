using Microsoft.AspNetCore.Mvc;

namespace ui.web.Controllers
{
    public class ValidationController : Controller
    {
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyEmail(string email)
        {
            //if (!_userRepository.VerifyEmail(email))
            //{
            //    return Json(data: $"Email {email} is already in use.");
            //}

            return Json(data: true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyUsername(string username)
        {
            //if (!_userRepository.VerifyEmail(email))
            //{
            //    return Json(data: $"Email {email} is already in use.");
            //}

            return Json(data: true);
        }
    }
}
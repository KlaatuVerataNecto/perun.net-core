using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class PasswordController : Controller
    {
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
            return View();
        }
    }
}
using infrastructure.user.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using peruncore.Infrastructure.Auth;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserAccountService _userAccountService;

        public SettingsController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
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

        public IActionResult ChangeEmail()
        {
            return View();
        }

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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using peruncore.Config;

namespace peruncore.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthSchemeSettings _authSchemeSettings;
        public UserController(IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _authSchemeSettings = authSchemeSettings.Value;
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Authentication.SignOutAsync(_authSchemeSettings.Application);
            HttpContext.Authentication.SignOutAsync(_authSchemeSettings.Google);
            HttpContext.Authentication.SignOutAsync(_authSchemeSettings.Application);
            return Redirect("/");
        }


        [Route("user/{id:int}/{username}")]
        [Authorize]
        public IActionResult Index(int id, string username)
        {
            return View();
        }
    }
}
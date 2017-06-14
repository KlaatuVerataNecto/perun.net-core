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
        public IActionResult Logout()
        {
            HttpContext.Authentication.SignOutAsync(_authSchemeSettings.Default);
            return Redirect("/");
        }


        [Route("user/{id:int}/{username}")]
        public IActionResult Index(int id, string username)
        {
            return View();
        }
    }
}
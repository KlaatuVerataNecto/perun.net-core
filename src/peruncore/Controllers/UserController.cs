using Microsoft.AspNetCore.Mvc;
using peruncore.Config;

namespace peruncore.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Authentication.SignOutAsync(ConfigVariables.AuthSchemeName);
            return Redirect("/");
        }


        [Route("user/{id:int}/{username}")]
        public IActionResult Index(int id, string username)
        {
            return View();
        }
    }
}
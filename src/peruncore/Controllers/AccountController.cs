using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class AccountController : Controller
    {
   
        [Authorize]
        public IActionResult Index()
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
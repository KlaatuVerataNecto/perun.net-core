using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class AccountController : Controller
    {
        // TODO: add [Authorize]
        public IActionResult avatar()
        {
            return View();
        }
    }
}
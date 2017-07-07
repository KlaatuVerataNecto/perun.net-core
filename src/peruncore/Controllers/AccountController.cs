using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult avatar()
        {
            return View();
        }
    }
}
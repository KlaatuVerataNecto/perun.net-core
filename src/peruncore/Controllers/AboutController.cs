using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
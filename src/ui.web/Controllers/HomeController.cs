using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;

namespace ui.web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //using (MiniProfiler.Current.Step("Example Step"))
            //{
            //    using (MiniProfiler.Current.Step("Sub timing"))
            //    {
            //        // Not trying to delay the page load here, only serve as an example
            //    }
            //    using (MiniProfiler.Current.Step("Sub timing 2"))
            //    {
            //        // Not trying to delay the page load here, only serve as an example
            //    }
            //}
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResetTokenExpired()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
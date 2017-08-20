using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace peruncore.Controllers
{
    public class PoliciesController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
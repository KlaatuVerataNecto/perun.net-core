using Microsoft.AspNetCore.Mvc;

namespace ui.web.Controllers
{
    public class ValidationController : Controller
    {
        public IActionResult isEmailAvailable(string email)
        {
            // TODO: check against database
            return Json("true");
            //return Json(_repository.CheckEmailExists(email) ?
            //        "true" : string.Format("an account for address {0} already exists.", email));
            //return Json(string.Format("an account for address {0} already exists.", email));
        }
    }
}
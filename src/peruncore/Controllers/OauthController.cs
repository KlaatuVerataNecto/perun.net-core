using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using peruncore.Config;

namespace peruncore.Controllers
{
    public class OauthController : Controller
    {
        public ActionResult google()
        {
            // Fix for OWIN bug
            // http://stackoverflow.com/questions/20737578/asp-net-sessionid-owin-cookies-do-not-send-to-browser/21234614#21234614
            // Session["RunSession"] = "1";
            return new ChallengeResult(ConfigVariables.AuthSchemeGoogle, new AuthenticationProperties { RedirectUri = "/oauth/callback" });
        }

        public ActionResult callback()
        {
            var authInfo = HttpContext.Authentication.GetAuthenticateInfoAsync(ConfigVariables.AuthSchemeGoogle).Result;
            return Content(authInfo.Principal.Identity.Name);
        }
    }
 }
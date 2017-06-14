using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using peruncore.Config;

namespace peruncore.Controllers
{
    public class OauthController : Controller
    {
        private readonly AuthSchemeSettings _authSchemeSettings;
        public OauthController(IOptions<AuthSchemeSettings> authSchemeSettings)
        {
            _authSchemeSettings = authSchemeSettings.Value;
        }

        public ActionResult google()
        {
            // Fix for OWIN bug
            // http://stackoverflow.com/questions/20737578/asp-net-sessionid-owin-cookies-do-not-send-to-browser/21234614#21234614
            // Session["RunSession"] = "1";
            return new ChallengeResult(_authSchemeSettings.Google, new AuthenticationProperties { RedirectUri = "/oauth/callback" });
        }

        public ActionResult callback()
        {
            var authInfo = HttpContext.Authentication.GetAuthenticateInfoAsync(_authSchemeSettings.Google).Result;
            return Content(authInfo.Principal.Identity.Name);
        }
    }
 }
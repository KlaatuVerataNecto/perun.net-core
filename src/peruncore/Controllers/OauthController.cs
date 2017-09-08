using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

using infrastructure.user.interfaces;
using peruncore.Config;
using peruncore.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using peruncore.Models.User;
using infrastructure.i18n.user;

namespace peruncore.Controllers
{
    public class OauthController : Controller
    {
        private readonly ISocialLoginService _socialLoginService;
        private readonly IUserAccountService _userAccountService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly IAuthProviderValidationService _authProviderValidationService;

        public OauthController(
              ISocialLoginService socialLoginService,
              IUserAccountService userAccountService,
              IOptions<AuthSchemeSettings> authSchemeSettings,
              IAuthProviderValidationService authProviderValidationService
            )
        {
            _authSchemeSettings = authSchemeSettings.Value;
            _socialLoginService = socialLoginService;
            _userAccountService = userAccountService;
            _authProviderValidationService = authProviderValidationService;
        }
        public ActionResult facebook()
        {
            return new ChallengeResult(
                _authSchemeSettings.Facebook,
                new AuthenticationProperties
                {
                    RedirectUri = "/oauth/callback/facebook"
                }
            );
        }

        public ActionResult google()
        {           
            return new ChallengeResult(
                _authSchemeSettings.Google, 
                new AuthenticationProperties {
                    RedirectUri = "/oauth/callback/google"
                }
            );
        }

        public ActionResult twitter()
        {
            return new ChallengeResult(
                _authSchemeSettings.Twitter,
                new AuthenticationProperties
                {
                    RedirectUri = "/oauth/callback/twitter"
                }
            );
        }

        [Route("oauth/callback/{provider}")]
        public ActionResult callback(string provider)
        {
            var authProvider = _authProviderValidationService.GetProviderName(provider);
            var authInfo = HttpContext.Authentication.GetAuthenticateInfoAsync(authProvider).Result;

            var currentIdentity = (ClaimsIdentity)User.Identity;
            var authIdentity = (ClaimsIdentity)authInfo.Principal.Identity;

            var userIdentity = _socialLoginService.loginOrSignup(
                authIdentity.GetSocialLoginUserId(),
                authIdentity.GetEmail(),
                authIdentity.GetFirstName(),
                authIdentity.GetLastName(),
                authProvider,
                (currentIdentity!= null)?currentIdentity.GetUserId():null
                );

            // TODO: Duplicated code
            HttpContext.Authentication.SignInAsync(
                _authSchemeSettings.Application,
                 ClaimsPrincipalFactory.Build(
                    userIdentity.UserId,
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar),
                new AuthenticationProperties
                {
                    IsPersistent = true

                }
            );

            if (userIdentity.IsRequiresNewUsername)
            {
                return RedirectToAction("Username", "Oauth");
            }

            return RedirectToAction("Index", "Home");          
        }

        [HttpGet]
        [Authorize]
        public IActionResult Username()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var tokenObj = _socialLoginService.getTokenByUserId(identity.GetUserId());
            return View(new UsernameModel {  userid = tokenObj.UserId , token = tokenObj.Token });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Username(UsernameModel model)
        {
            if (!ModelState.IsValid) return View("Username", model);

            var identity = (ClaimsIdentity)User.Identity;

            if (identity.GetUserId() != model.userid)
            {
                // TODO: redirect to error session expired
            }

            var userIdentity = _userAccountService.changeUsername(model.userid, model.username, model.token);

            if (userIdentity == null)
            {
                ModelState.AddModelError("username", UserValidationMsg.username_not_available);
                return View("Username", model);
            }

            // TODO: Duplicated code
            HttpContext.Authentication.SignInAsync(
                _authSchemeSettings.Application,
                 ClaimsPrincipalFactory.Build(
                    userIdentity.UserId,
                    userIdentity.Username,
                    userIdentity.Email,
                    userIdentity.Roles,
                    userIdentity.Avatar),
                new AuthenticationProperties
                {
                    IsPersistent = true

                }
            );

            return RedirectToAction("Index","Home");
        }

    }
}
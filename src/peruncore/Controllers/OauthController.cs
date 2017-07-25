using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

using infrastructure.user.interfaces;
using peruncore.Config;
using peruncore.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using peruncore.Models.User;

namespace peruncore.Controllers
{
    public class OauthController : Controller
    {
        private readonly ISocialLoginService _socialLoginService;
        private readonly AuthSchemeSettings _authSchemeSettings;
        private readonly IAuthProviderValidationService _authProviderValidationService;

        public OauthController(
              ISocialLoginService socialLoginService,
              IOptions<AuthSchemeSettings> authSchemeSettings,
              IAuthProviderValidationService authProviderValidationService
            )
        {
            _authSchemeSettings = authSchemeSettings.Value;
            _socialLoginService = socialLoginService;
            _authProviderValidationService = authProviderValidationService;
        }
        public ActionResult facebook()
        {
            return new ChallengeResult(
                _authSchemeSettings.Google,
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

        [Route("oauth/callback/{provider}")]
        public ActionResult callback(string provider)
        {
            var authProvider = _authProviderValidationService.GetProviderName(provider);

            var authInfo = HttpContext.Authentication.GetAuthenticateInfoAsync(authProvider).Result;

            var identity = (ClaimsIdentity)authInfo.Principal.Identity;

            var userIdentity = _socialLoginService.loginOrSignup(
                identity.GetUserId(),
                identity.GetEmail(),
                identity.GetFirstName(),
                identity.GetLastName(),
                authProvider
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
            // TODO: Hmm don't like the parsing part of userid
            var userIdentity = (ClaimsIdentity)User.Identity;
            var tokenObj = _socialLoginService.getTokenByUserId(int.Parse(userIdentity.GetUserId()));
            return View(new UsernameModel {  userid = tokenObj.UserId , token = tokenObj.Token });
        }
    }
 }
using System.Linq;
using System.Security.Claims;

namespace peruncore.Infrastructure.Auth
{
    public static class SpecificClaimExtension
    {
        public static string GetUserName(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetSocialLoginUserId(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return (claim != null) ? claim.Value : string.Empty;
        }

        public static int GetUserId(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.FindFirst("userid");

            return (claim != null) ? int.Parse(claim.Value) : 0;
        }

        public static int GetLoginId(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.FindFirst("loginid");

            return (claim != null) ? int.Parse(claim.Value) : 0;
        }

        public static string GetEmail(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetFirstName(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName);

            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetLastName(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetProvider(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.FindFirst("provider");
            return (claim != null) ? claim.Value : null;
        }

        public static string GetRoles(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetAvatar(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.FindFirst("avatar");
            return (claim != null) ? claim.Value : null;
        }
    }
}

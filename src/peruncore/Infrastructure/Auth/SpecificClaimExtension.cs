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
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

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
    }

    //public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
    //{
    //    var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

    //    return (claim != null) ? claim.Value : string.Empty;
    //}
}

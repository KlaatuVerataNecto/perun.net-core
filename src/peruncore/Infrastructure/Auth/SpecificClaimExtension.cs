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

        public static string GetUserId(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return (claim != null) ? claim.Value : string.Empty;
        }

    }

    //public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
    //{
    //    var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

    //    return (claim != null) ? claim.Value : string.Empty;
    //}
}

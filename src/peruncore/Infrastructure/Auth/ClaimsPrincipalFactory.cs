using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace peruncore.Infrastructure.Auth
{
    public static class ClaimsPrincipalFactory
    {

        public static ClaimsPrincipal Build(int userid, string username, string email, string rolenames, string avatar)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userid.ToString()));
            claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim(ClaimTypes.Email, email));

            if (!String.IsNullOrEmpty(avatar))
            {
                claims.Add(new Claim("Avatar", avatar));
            }

            if (!String.IsNullOrEmpty(rolenames))
            {
                List<string> roles = rolenames.Split(',').ToList<string>();
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var id = new ClaimsIdentity(claims, "local", "name", "role");
            return new ClaimsPrincipal(id);
        }

    }
}

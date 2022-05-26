using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Unite.Identity.Entities;

namespace Unite.Composer.Web.Controllers.Identity.Helpers
{
    public class ClaimsHelper
    {
        public static ClaimsIdentity GetIdentity(User user)
        {
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email)
            });

            return identity;
        }

        public static string GetValue(IEnumerable<Claim> claims, string name)
        {
            return claims.FirstOrDefault(claim => claim.Type == name)?.Value;
        }
    }
}

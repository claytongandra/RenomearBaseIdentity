using System.Security.Claims;
using System.Security.Principal;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.Configuration
{
    public static class IdentityExtensions
    {

        public static string GetInfoUser(this IPrincipal user, string claimName)
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == claimName)
                            return claim.Value;
                    }
                }
                return "";
            }
            else
                return "";
        }
    }
}

using System.Linq;
using System.Security.Claims;

namespace ApiProtection.MedicalPrescriptionsApi.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        public static string GetSubject(this ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal == null || claimsPrincipal.Identity == null || !claimsPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return null;
            }

            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "sub");
            return claim == null ? null : claim.Value;
        }
    }
}

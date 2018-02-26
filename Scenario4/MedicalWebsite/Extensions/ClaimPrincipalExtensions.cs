using System.Security.Claims;

namespace MedicalWebsite.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetRole(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role);
        }

        public static string GetSubject(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject);
        }

        public static string GetGivenName(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName);
        }

        public static string GetClaimValue(ClaimsPrincipal principal, string claimName)
        {
            if (principal == null ||
                principal.Identity == null)
            {
                return null;
            }

            var claim = principal.FindFirst(claimName);
            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }
    }
}

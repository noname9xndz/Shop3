using Shop3.Utilities.Constants;
using System;
using System.Linq;
using System.Security.Claims;

namespace Shop3.Extensions
{
    public static class IdentityExtensions
    {
        // get tt tin từ claim
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

        // get tt user thông qua claim UserId trong Shop3.Helpers.CustomClaimsPrincipalFactory
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == CommonConstants.UserClaims.UserId); // ép kiểu về claimsIdentity query đến type = UserId lúc login
            return Guid.Parse(claim.Value);
        }
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        { // ép kiểu về claimsIdentity query đến type = UserId lúc login
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == CommonConstants.UserClaims.Email);
            return claim.Value;
        }
    }
}
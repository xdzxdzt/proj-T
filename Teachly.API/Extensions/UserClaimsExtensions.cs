using System.Security.Claims;

namespace Teachly.API.Extensions
{
    public static class UserClaimsExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
        {
            var userIdClaim = user.FindFirstValue("userId");

            return Guid.TryParse(userIdClaim, out userId);
        }
    }
}

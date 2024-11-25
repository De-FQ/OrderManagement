using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsExtension
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}

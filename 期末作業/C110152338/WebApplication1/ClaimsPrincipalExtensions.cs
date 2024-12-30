using System.Security.Claims;

namespace WebApplication1
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value; // 確保不會因為 null 導致例外
        }
    }
}

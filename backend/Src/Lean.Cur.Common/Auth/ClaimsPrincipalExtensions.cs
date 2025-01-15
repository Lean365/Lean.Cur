using System.Security.Claims;

namespace Lean.Cur.Common.Auth
{
    /// <summary>
    /// ClaimsPrincipal扩展方法
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="principal">ClaimsPrincipal</param>
        /// <returns>用户ID</returns>
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            {
                return 0;
            }
            return userId;
        }
    }
} 
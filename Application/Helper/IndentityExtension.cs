using Domain.Constants.Authenticate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Helper
{
    public static class IndentityExtension
    {
        //get, extract userId in jwt token 
        public static string GetUserIdFromToken(this IPrincipal user)
        {
            if (user == null)
                return string.Empty;

            var identity = user.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity!.Claims;
            return claims.FirstOrDefault(s => s.Type == "UserId")?.Value ?? string.Empty;
        }
        

        public static string GetEmailFromToken(this IPrincipal user)
        {
            if (user == null)
                return string.Empty;

            var identity = user.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity!.Claims;
            return claims.FirstOrDefault(s => s.Type.Equals(UserClaimType.Email))?.Value ?? string.Empty;
        }

        private static string? GetJwtTokenFromHeader(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                // Giả sử token được truyền trong header với tiền tố "Bearer "
                return authHeader.FirstOrDefault()?.Replace("Bearer ", "");
            }

            return string.Empty;
        }
    }
}

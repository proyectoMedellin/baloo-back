using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SiecaAPI.Controllers
{
    public static class ControllerTools
    {
        public static string GetRequestUserName(HttpContext httpContext)
        {
            string requestUserName = string.Empty;

            if (httpContext != null)
            {
                ClaimsIdentity? identity = httpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    List<Claim> claims = identity.Claims.ToList();
                    List<Claim> userNameClaims = claims.Where(c => c.Type.Equals(ClaimTypes.Name)).ToList();
                    if (userNameClaims.Count > 0)
                    {
                        requestUserName = userNameClaims[0].Value;
                    }
                }
            }
            return requestUserName;
        }
    }
}

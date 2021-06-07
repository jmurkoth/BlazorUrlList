using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JM.BlzrUrlList.Web
{
    public static class UserClaimHelper
    {
        public static string GetUserNameClaim(this ClaimsPrincipal principal)
        {
            string userName=string.Empty;
    
            userName= principal?.Claims?.FirstOrDefault(c=>c.Type=="preferred_username")?.Value;
            
            return userName;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace _95PhrEAKer.Services.helpers
{
    public class GetTokeDetails
    {
        [Authorize]
        public static string GetEmailIdFromToken(ClaimsPrincipal User)
        {
            var userIdClaim = User.Claims;
            string email = userIdClaim.FirstOrDefault(x => x.Type=="Email").ToString();
            return email.Substring(7);
        }
    }
}

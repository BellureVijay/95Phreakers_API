using _95PhrEAKer.Persistence.DbModals;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

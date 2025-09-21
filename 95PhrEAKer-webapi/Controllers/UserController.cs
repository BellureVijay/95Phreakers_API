using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Services.helpers;
using _95PhrEAKer.Services.IServices.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        public UserController(IUser user)
        {
            _user = user;
        }
        private string GetEmailFromToken()
        {
            return GetTokeDetails.GetEmailIdFromToken(User);
        }
        [HttpGet("/GetAllUsers")]
        [Authorize]
        public List<User> GetAllUsers()
        {
            string email = GetEmailFromToken();
            return _user.GetAllUsers(email);
        }
        
    }
}

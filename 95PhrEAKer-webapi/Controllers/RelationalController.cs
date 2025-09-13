using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Services.helpers;
using _95PhrEAKer.Services.IServices.RelationalService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationalController : ControllerBase
    {
        private readonly IRelationalService _relations;
        private string GetEmailFromToken()
        {
            return GetTokeDetails.GetEmailIdFromToken(User);
        }
        public RelationalController(IRelationalService Relations)
        {
            _relations = Relations;
        }
        [HttpPost("/sendRequest")]
        [Authorize]
        public async Task<IActionResult> SendRequest(string recieverName)
        {
            string email = GetEmailFromToken();
            await _relations.SendRequest(recieverName,email);
            return Ok("Request sent successfully.");
        }
        [HttpPost("/acceptRequest")]
        [Authorize]
        public async Task<IActionResult> AcceptRequest(string recieverName)
        {
            string email = GetEmailFromToken();
            await _relations.AcceptRequest(recieverName,email);
            return Ok("Request Accepted successfully.");
        }

        [HttpGet("/Requests")]
        [Authorize]
        public  List<User> ActiveRequests()
        {
            string email = GetEmailFromToken();
            return _relations.GetRequests(email);
        }       
        [HttpGet("/RequestSent")]
        [Authorize]
        public  List<User> ActiveRequestsent()
        {
            string email = GetEmailFromToken();
            return _relations.GetRequestSent(email);
        }
        
        [HttpGet("/GetFriends")]
        [Authorize]
        public  List<User> GetFriends()
        {
            string email = GetEmailFromToken();
            return _relations.GetFriends(email);
        }
    }
}

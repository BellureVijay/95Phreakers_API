using _95PhrEAKer.Domain.UserPost;
using _95PhrEAKer.Persistence.DbModals;
using _95PhrEAKer.Services.helpers;
using _95PhrEAKer.Services.IServices.UserPostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPostsController : ControllerBase
    {
        private readonly IUserPostService _userPostService;
        private string GetEmailFromToken()
        {
            return  GetTokeDetails.GetEmailIdFromToken(User);
        }
        public UserPostsController(IUserPostService userPostService)
        {
            _userPostService = userPostService;
        }
        [HttpPost("/CreatePost")]
        public async Task<IActionResult> CreatePost([FromQuery] string message, [FromQuery] string email)
        {
            await _userPostService.CreatePost(message,email);
            return Ok("Email sent successfully.");
        }
        [HttpGet("/getMyPosts")]
        [Authorize]
        public  List<Post> GetMyPosts()
        {
            string email = GetEmailFromToken();
            var item=_userPostService.GetMyPosts(email);
            return item;
        }

        [HttpDelete("/DeletePost/{postId}")]
        [Authorize]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                _userPostService.DeletMyPost(postId);
                return Ok("post deleted successfully");
            }
            catch(Exception e)
            {
                throw new Exception("Failed to delete this post");
            }
        }

    }
}

using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Domain.UserPost;
using Microsoft.AspNetCore.Mvc;

namespace _95PhrEAKer.Services.IServices.UserPostService
{
    public interface IUserPostService
    {
        List<Post> GetMyPosts(string email);
        Task<IActionResult> CreatePost(string message, string email);
        void DeletMyPost(int postid);

        List<DashBoardPost> GetDashboardPosts(string email);
    }
}

using _95PhrEAKer.Domain.EmailSetting;
using _95PhrEAKer.Domain.UserPost;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Persistence.DbModals;
using _95PhrEAKer.Services.IServices.UserPostService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace _95PhrEAKer.Services.ServicesExtension.UserPostService
{
    public class UserPostsService : IUserPostService
    {
        private readonly Post _posts;
        private readonly AppDbContext _context;

        public UserPostsService(IOptions<Post> posts, AppDbContext context)
        {
            _posts = posts.Value;
            _context = context;
        }

        public async Task<IActionResult> CreatePost(string message, string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            var userPost = new Persistence.DbModals.Posts
            {
                Message = message,
                Users = user,
                CreatedDate = DateTime.Now
            };

            _context.Posts.Add(userPost);
            await _context.SaveChangesAsync();

            return new OkObjectResult(userPost);
        }

        public List<Post> GetMyPosts(string email)
        {
            
            var userId=_context.Users.FirstOrDefault(x=>x.Email==email).ID;
            List<Post> PostResult = new List<Post>();
            var allPosts = _context.Posts.Where(x=>x.UserId==userId).OrderByDescending(x=>x.ID).ToList();

            foreach (var item in allPosts)
            {
                var p = new Post
                {
                    PostId = item.ID,
                    Message = item.Message,
                    CreatedDate=item.CreatedDate
                };
                PostResult.Add(p);
            }
            return PostResult;
        }

        public  void DeletMyPost(int postid)
        {
            var deletionPost = _context.Posts.FirstOrDefault(x => x.ID == postid);
            if (deletionPost != null)
            {
            _context.Posts.Remove(deletionPost);
            }
            _context.SaveChanges();
        }
    }
}

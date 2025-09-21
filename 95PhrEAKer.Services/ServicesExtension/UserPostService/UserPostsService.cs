using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Domain.UserPost;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Services.IServices.UserPostService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

        public List<DashBoardPost> GetDashboardPosts(string email)
        {
            List<DashBoardPost> PostResult = new List<DashBoardPost>();
            // ✅ Step 1: Get the user ID from email
            var userId = _context.Users
                .Where(x => x.Email == email)
                .Select(x => x.ID)
                .FirstOrDefault();

            if (userId == 0)
            {
                // Handle case where user is not found
                return new List<DashBoardPost>();
            }

            // ✅ Step 2: Define the cutoff date (2 days ago)
            var cutoffDate = DateTime.Now.AddDays(-2);

            // ✅ Step 3: Get user's posts older than 2 days
            var myPosts = _context.Posts
                .Where(x => x.UserId == userId && x.CreatedDate >=cutoffDate)
                .OrderByDescending(x => x.ID)
                .ToList();

            // ✅ Step 4: Get IDs of user's friends
            var friendsIds = _context.Relations
                .Where(r => r.RelationalStatus == true && (r.SenderID == userId || r.ReceiverID == userId))
                .Select(r => r.SenderID == userId ? r.ReceiverID : r.SenderID)
                .Distinct()
                .ToList();

            // ✅ Step 5: Get friends' posts older than 2 days
            var friendsPosts = _context.Posts
                .Where(x => friendsIds.Contains(x.UserId) && x.CreatedDate >= cutoffDate)
                .OrderByDescending(x => x.ID)
                .ToList();

            // ✅ Step 6: Combine and sort all posts
            var allPosts = myPosts
                .Concat(friendsPosts)
                .OrderByDescending(x => x.ID)
                .ToList();
            foreach (var item in allPosts)
            {
                var p = new Post
                {
                    PostId = item.ID,
                    Message = item.Message,
                    CreatedDate = item.CreatedDate
                };
                var userName = _context.Users.FirstOrDefault(u => u.ID == item.UserId)?.UserName;
                var dashBoardPost = new DashBoardPost
                {
                    PostDetail = p,
                    UserName = userName
                };
                PostResult.Add(dashBoardPost);
            }
            return PostResult;
        }
    }
}

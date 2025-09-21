using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Persistence.DbModals;
using _95PhrEAKer.Services.IServices.RelationalService;
using _95PhrEAKer.Services.ServicesExtension.ChatServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace _95PhrEAKer.Services.ServicesExtension.RelationalServices
{
    public class RelationalService : IRelationalService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ChathubService> _hubContext;
        public RelationalService(AppDbContext context,IHubContext<ChathubService> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<User> GetRequests(string email)
        {
            var userId = _context.Users.FirstOrDefault(x => x.Email == email).ID;

            var requests = _context.Users
                .Where(u => _context.Relations
                    .Any(r =>( r.SenderID == u.ID && r.ReceiverID == userId) && r.RelationalStatus==false));


            if (requests == null)
            {
                return null;
            }
            else
            {
                List<User> users = new List<User>();
                foreach(var item in requests)
                {
                    var user = new User
                    {
                        userId = item.ID,
                        userName = item.UserName
                    };

                    users.Add(user);
                }
                return users;
            }
        }

        public List<User> GetRequestSent(string email)
        {
            var userId = _context.Users.FirstOrDefault(x => x.Email == email).ID;

            var requests = _context.Users
                .Where(u => _context.Relations
                    .Any(r => (r.SenderID == userId && r.ReceiverID == u.ID) && r.RelationalStatus == false));


            if (requests == null)
            {
                return null;
            }
            else
            {
                List<User> users = new List<User>();
                foreach (var item in requests)
                {
                    var user = new User
                    {
                        userId = item.ID,
                        userName = item.UserName
                    };

                    users.Add(user);
                }
                return users;
            }
        }

        public List<User> GetFriends(string email)
        {
            // 1. Safely retrieve the user and handle cases where the user is not found.
            // If the user doesn't exist, userId will be null.
            var userId = _context.Users.FirstOrDefault(x => x.Email == email)?.ID;

            // Handle the case where the user is not found, return an empty list of friends.
            if (!userId.HasValue)
            {
                return new List<User>();
            }

            // 2. Identify the IDs of the user's friends.
            // This query gets the relationships where the current user is involved.
            // Then, it projects the ID of the 'other' user in each relationship and ensures uniqueness.
            var friendIds = _context.Relations
                .Where(r => r.RelationalStatus == true && (r.SenderID == userId || r.ReceiverID == userId))
                .Select(r => r.SenderID == userId ? r.ReceiverID : r.SenderID)
                .Distinct()
                .ToList();

            // 3. Retrieve the friend user objects and project directly into the desired User format.
            var friends = _context.Users
                .Where(u => friendIds.Contains(u.ID)) // Filter users where ID is in the friendIds list
                .Select(u => new User
                {
                    userId = u.ID,        // Assuming User has an ID property
                    userName = u.UserName // Assuming User has a UserName property
                })
                .ToList();

            return friends;
        }


        public async Task<IActionResult> SendRequest(string recieverName,string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            var reciever = _context.Users.FirstOrDefault(x => x.UserName == recieverName);
            if (user == null || reciever==null)
            {
                return new NotFoundObjectResult("User not found.");
            }
            else
            {
                var connection = new Relation
                {
                    SenderID = user.ID,
                    ReceiverID = reciever.ID,
                    CreatedDate = DateTime.Now,
                    RelationalStatus = false,
                };
                _context.Relations.Add(connection);


                var notification = new Notifications
                {
                    UserId = reciever.ID.ToString(),
                    Message = $"You have a new friend request from {user.UserName}",
                    Type = "friend_request",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.User(reciever.ID.ToString())
                            .SendAsync("ReceiveNotification", notification.Message);

                return new OkObjectResult(connection);
            }
        }

        public async Task<IActionResult> AcceptRequest(string recieverName, string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            var reciever = _context.Users.FirstOrDefault(x => x.UserName == recieverName);
            if (user == null || reciever == null)
            {
                return new NotFoundObjectResult("User not found.");
            }
            else
            {
                _context.Relations.FirstOrDefault(x => x.SenderID == reciever.ID && x.ReceiverID == user.ID).RelationalStatus = true;
                await _context.SaveChangesAsync();

                return new OkResult();
            }
        }
    }
}

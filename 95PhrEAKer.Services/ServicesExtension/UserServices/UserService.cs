using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Services.IServices.UserServices;
using Microsoft.EntityFrameworkCore;

namespace _95PhrEAKer.Services.ServicesExtension.UserServices
{
    public class UserService : IUser
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers(string email)
        {
            List<User> UsersModal = new List<User>();
            var userID = _context.Users.FirstOrDefault(x => x.Email == email).ID;
            var recivers = _context.Relations.AsNoTracking().Where(x => x.SenderID==userID).Select(x=>x.ReceiverID).ToList();
            var senders = _context.Relations.AsNoTracking().Where(x => x.ReceiverID == userID).Select(x=>x.SenderID).ToList();
            senders.AddRange(recivers);
            var users = _context.Users.AsNoTracking().Where(x => x.Email != email && !senders.Contains(x.ID));

            foreach (var user in users){
                var usermodal = new User
                {
                    userId = user.ID,
                    userName = user.UserName
                };
                UsersModal.Add(usermodal);
            }
            return UsersModal;
        }
    }
}

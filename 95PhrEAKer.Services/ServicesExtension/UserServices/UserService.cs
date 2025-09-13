using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Services.IServices.UserPostService;
using _95PhrEAKer.Services.IServices.UserServices;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var senders = _context.Relations.AsNoTracking().Select(x => x.SenderID).ToList();
            var recivers = _context.Relations.AsNoTracking().Select(x => x.ReceiverID).ToList();
            senders.AddRange(recivers);
            var users = _context.Users.AsNoTracking().Where(x=>x.Email!=email && !senders.Contains(x.ID));

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

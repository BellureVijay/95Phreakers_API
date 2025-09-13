using _95PhrEAKer.Domain.User;
using _95PhrEAKer.Persistence.DbModals;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _95PhrEAKer.Services.IServices.RelationalService
{
    public interface IRelationalService
    {
        Task<IActionResult> SendRequest(string recieverName,string email);
        Task<IActionResult> AcceptRequest(string recieverName,string email);

        List<User> GetRequests(string email);
        List<User> GetRequestSent(string email);
        List<User> GetFriends(string email);
    }
}

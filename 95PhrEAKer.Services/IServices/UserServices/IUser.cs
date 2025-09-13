using _95PhrEAKer.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _95PhrEAKer.Services.IServices.UserServices
{
    public interface IUser
    {
        List<User> GetAllUsers(string email);
    }
}

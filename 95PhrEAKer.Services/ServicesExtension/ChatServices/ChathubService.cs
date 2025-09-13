using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace _95PhrEAKer.Services.ServicesExtension.ChatServices
{
    public class ChathubService:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }
}

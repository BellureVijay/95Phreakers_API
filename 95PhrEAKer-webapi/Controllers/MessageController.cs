using _95PhrEAKer.Services.ServicesExtension.ChatServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<ChathubService> _hubContext;

        public MessageController(IHubContext<ChathubService> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("/Message")]
        [Authorize]
        public IActionResult Post([FromBody] MessageDto message)
        {
            // Send the message to all connected clients
            _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
            return Ok();
        }

        [HttpPost("/PrivateMessage")]
        [Authorize]
        public IActionResult SendPrivate([FromBody] PrivateMessageDto message)
        {
            // Send the message only to the specified recipient
            _hubContext.Clients.User(message.Recipient).SendAsync("ReceivePrivateMessage", message.User, message.Text);
            return Ok();
        }
    }
    public class MessageDto
    {
        public string User { get; set; }
        public string Text { get; set; }
    }

    public class PrivateMessageDto
    {
        public string User { get; set; }
        public string Recipient { get; set; }
        public string Text { get; set; }
    }
}

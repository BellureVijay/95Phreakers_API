using Microsoft.AspNetCore.Mvc;
using _95PhrEAKer.Domain.WebHook;
using Microsoft.AspNetCore.Authorization;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHook : ControllerBase
    {

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebHookPayload payload)
        {
               
            return Ok();
        }

    }
}

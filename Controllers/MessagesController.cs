using Microsoft.AspNetCore.Mvc;
using Aspcorespa.Context;
using Aspcorespa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Aspcorespa.Controllers
{
    [Produces("application/json")]
    [Route("api/messages")]
    [Authorize(AuthenticationSchemes = "Cookie,Bearer")]
    public class MessagesController : Controller
    {
        private readonly IMessageRepository context;
        private readonly UserManager<UserEntity> _usermanger;


        public MessagesController(IMessageRepository r, UserManager<UserEntity> usermanger)
        {
            context = r;
            _usermanger = usermanger;
        }

        private async Task<UserViewModel> GetCurrentUserAccount()
        {
            var user = await _usermanger.FindByIdAsync(HttpContext.User.Claims.First().Value);

            return new UserViewModel { Name = user.Name, Username = user.UserName, Description = user.Description, Email = user.Email, ImageUrl = user.ImageUrl };
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var user = await GetCurrentUserAccount();
            if (user == null) return NoContent();
            var messages = context.GetAllMessage(user.Username);
            return Ok(messages);
        }

        [HttpPost("visibility")]
        public async Task<IActionResult> ChangeMessageVisibility([FromBody] Message message)
        {
            if (message == null || message.ID == 0) return BadRequest(new { message = "please provide Message ID" });
            var user = await GetCurrentUserAccount();
            if (!context.ChangeMessageVisibility(message.ID, user.Username))
            {
                return BadRequest(new { message = "something wrong, this message maybe not yours" });
            }
            return Ok();

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage( int id )
        {
            if (id == 0) return BadRequest(new { message = "please provide Message ID" });
            var user = await GetCurrentUserAccount();
            if (!context.DeleteMessage(id, user.Username))
            {
                return BadRequest(new { message = "something wrong, you need to delete another messages" });
            }
            return Ok();

        }

        [HttpGet("reply")]
        public IActionResult GetSingleReply(int id)
        {
            if (id == 0) return BadRequest(new { message = "please provide Message ID" });
            var reply = context.GetSingleMessageReply(id);
            return Ok(new { reply = reply });
        }

        [HttpPost("reply")]
        public IActionResult PostReply([FromBody]Message message)
        {
            if (message.ID == 0 || message.Reply == null) return BadRequest(new { message = "please provide Message ID and reply" });
           if (!context.AddReply(message.ID, message.Reply))
            {
                return BadRequest();
            }
            return Ok();
        }


    }
}

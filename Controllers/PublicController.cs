using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aspcorespa.Context;
using Aspcorespa.Models;

namespace Aspcorespa.Controllers
{
    [Produces("application/json")]
    [Route("api/public")]
    public class PublicController : Controller
    {
        private readonly IMessageRepository mRepo;
        private readonly IUserRepository userRepo;
        public PublicController(IMessageRepository r, IUserRepository userR)
        {
            mRepo = r;
            userRepo = userR;
        }

        [HttpPost("message")]
        public IActionResult PostMessage([FromBody]MessageRequestModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Message)) return BadRequest();
            if (mRepo.AddMessage(model))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("user")]
        public IActionResult GetUserDetails(string username)
        {
            if (username == null || string.IsNullOrWhiteSpace(username)) return BadRequest(new { message = "Provide username" });
            var user = userRepo.GetUserViewDetails(username);
            if (user == null) return BadRequest(new { message = "user notfound" });
            return Ok(user);
        }

        [HttpGet("user/messages")]
        public IActionResult GetUserPublicMessages(string username)
        {
            if (username == null || string.IsNullOrWhiteSpace(username)) return BadRequest(new { message = "Provide username" });
            var message = mRepo.GetAllVisibleMessages(username);
            if (message == null) return Ok(new { message = "Messages notfound" });
            return Ok(message);
        }

    }
}
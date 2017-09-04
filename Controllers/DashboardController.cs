using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aspcorespa.Models;
using Microsoft.AspNetCore.Identity;
using Aspcorespa.Context;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Aspcorespa.ExtOperations;

namespace Aspcorespa.Controllers
{
    [Produces("application/json")]
    [Route("api/dashboard")]
    [Authorize(AuthenticationSchemes  = "Cookie,Bearer")]

    public class DashboardController : Controller
    {
        private readonly UserManager<UserEntity> _usermanger;

        private readonly IMessageRepository messageRepo;

        private readonly IUserRepository userRepo;

        private IHostingEnvironment _environment;


        public DashboardController(UserManager<UserEntity> usermanger, IMessageRepository context,
            IHostingEnvironment environment, IUserRepository userRepo)
        {
            _usermanger = usermanger;
            this.messageRepo = context;
            _environment = environment;
            this.userRepo = userRepo;

        }

        private async Task<UserViewModel> GetCurrentUserAccount()
        {
            var user = await _usermanger.FindByIdAsync(HttpContext.User.Claims.First().Value);

            return new UserViewModel { Name = user.Name, Username = user.UserName, Description = user.Description, Email = user.Email, ImageUrl = user.ImageUrl };
        }


        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUserAccount();
            if (user == null) return NoContent();
            var MessageCount = messageRepo.MessageCount(user.Username);
            return Ok(new
            {
                messagesCount = MessageCount,
                user = user

            });
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserDetails([FromForm]UserViewModel model, [FromForm] IFormFile image)
        {
            if (model == null) return BadRequest();
            if(image != null)
            {
                var imgPath = await FileOperations.SaveImageAsync(image, _environment);
                model.ImageUrl = imgPath;

                var currUserDetails = await GetCurrentUserAccount();
                if(currUserDetails.ImageUrl != null)
                {
                    var currentImage = currUserDetails.ImageUrl;
                    FileOperations.DeleteImage(currentImage);
                }
            }
            var user = userRepo.UpdateUserDetails(model);
            if (user == null) return BadRequest();
            return Ok(user);
        }

        [HttpPut("reset")]
        public async Task<IActionResult> UpdateUserPassword([FromForm]string oldPassword, [FromForm] string newPassword)
        {
            if (oldPassword == null || newPassword == null) return BadRequest();
            var user = await GetCurrentUserAccount();
            var output = await userRepo.ChangeUserPassword(user.Username, oldPassword, newPassword);
            if (!output) return BadRequest();
            return Ok();

        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Aspcorespa.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Aspcorespa.Context;
using Microsoft.EntityFrameworkCore;

namespace Aspcorespa.Controllers
{
    [Produces("application/json")]
    [Route("api/authapi")]
    public class AuthApiController : Controller
    {

        private readonly UserManager<UserEntity> _userManager;
        //private readonly SignInManager<UserEntity> _signInManager;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHostingEnvironment _environment;
        private readonly AppDBContext context;

        public AuthApiController(UserManager<UserEntity> userManager, IPasswordHasher<UserEntity> passwordHasher,
                                 RoleManager<IdentityRole> roleManager, IHostingEnvironment environment, AppDBContext context)
        {
            _userManager = userManager;
            ////_signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _environment = environment;
            this.context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromForm]UserViewModel model, [FromForm] IFormFile image)
        {

            if (Request.Form.Files.Count < 1) return BadRequest(new { message = "No Image Found" });
            #region OldCode
            //var imageFile = Request.Form.Files[0];

            ////var model = new UserViewModel();

            //Request.Form.TryGetValue("name", out var name);
            //model.Name = name[0];

            //Request.Form.TryGetValue("username", out var username);
            //model.Username = username[0];

            //Request.Form.TryGetValue("email", out var email);
            //model.Email = email[0];

            //Request.Form.TryGetValue("password", out var password);
            //model.Password = password[0];

            //Request.Form.TryGetValue("description", out var description);
            //model.Description = description[0];

            //model.ImageUrl = "http";
            #endregion

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToList());
            }
            if (context.Users.Any(u => u.Email == model.Email)) return BadRequest(new { message="Email already exsists" });


            var folderName = Path.Combine(_environment.WebRootPath, "uploads");
            var extension = Path.GetExtension(image.FileName);
            var imgPath = Path.Combine(folderName, $"{Path.GetRandomFileName()}{extension}");
            using (var fileStream =new FileStream(imgPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            
            var user = new UserEntity { UserName = model.Username, Email = model.Email, Name = model.Name, ImageUrl=imgPath, Description = model.Description };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description).ToList());
            }

            await _userManager.AddToRoleAsync(user, "user");

            //await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null) return BadRequest(new { message="Empty Content" });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return BadRequest(new { message="Email or Password Wrong." });
            }

            var userRole = await _userManager.GetRolesAsync(user);
            

            var token = CreateJwtPacket(user,userRole);
            var User = new UserViewModel{ Name=user.Name,Username=user.UserName,Description = user.Description, Email=user.Email,ImageUrl = user.ImageUrl};

            //Set Token with HTTPOnly from Server Side 
            Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true, Secure = true});
           
            return Ok(new
            {
                 token,
                User,
                Role = userRole.FirstOrDefault()
            });
          
        }

        [HttpDelete("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok();
        }


        string CreateJwtPacket(UserEntity user, IList<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("I Like to eat, like food."));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            if (roles != null)
                foreach (var role in roles)
                    claims.Add(new Claim("role", role));

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;

        }
    }
}

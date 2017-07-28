using Aspcorespa.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Aspcorespa.Context
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext dbContext;
        private readonly UserManager<UserEntity> _userManager;

        public UserRepository(AppDBContext dbContext, UserManager<UserEntity> userManager)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
        }

        public UserViewModel GetUserViewDetails(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return null;
            return new UserViewModel { Name = user.Name, ImageUrl = user.ImageUrl, Description = user.Description, Username = user.UserName };
        }

        public UserViewModel UpdateUserDetails(UserViewModel inUser)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == inUser.Username);
            if (user == null) return null;
            user.Name = inUser.Name ?? inUser.Name;
            user.ImageUrl = inUser.ImageUrl ?? inUser.ImageUrl;
            user.Description = inUser.Description ?? inUser.Description;
            dbContext.Update(user);
            dbContext.SaveChanges();
            return new UserViewModel { Name = user.Name, ImageUrl = user.ImageUrl, Description = user.Description, Username = user.UserName };
        }

        public async Task<bool> ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return false;
            var x = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!x.Succeeded) return false;
            return true;
        }
    }
}

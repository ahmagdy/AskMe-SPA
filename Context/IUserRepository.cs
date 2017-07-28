using System.Threading.Tasks;
using Aspcorespa.Models;

namespace Aspcorespa.Context
{
    public interface IUserRepository
    {
        Task<bool> ChangeUserPassword(string username, string oldPassword, string newPassword);
        UserViewModel GetUserViewDetails(string username);
        UserViewModel UpdateUserDetails(UserViewModel inUser);
    }
}
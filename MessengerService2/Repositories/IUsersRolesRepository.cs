using MessengerService2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerService2.Repositories
{
    public interface IUsersRolesRepository
    {
        Task<Users> GetUserAsync(string username);
        Users GetUser(string username);
        Task<bool> AddUserAsync(string username, string password);
        Task<bool> DeleteUserAsync(string username);
        Task<Users> ValidateUserAsync(string username, string password);
        Users ValidateUser(string username, string password);


        void RemoveAllRoles(int id);
        Task<bool> RemoveRolesAsync(string username, List<string> roles);
        Task<bool> AddRolesAsync(string username, List<string> roles);
    }
}

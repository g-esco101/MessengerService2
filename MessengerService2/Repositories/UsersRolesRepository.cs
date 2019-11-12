using MessengerService2.Helpers;
using MessengerService2.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerService2.Repositories
{
    public class UsersRolesRepository : IUsersRolesRepository
    {
        private MessengerService_DBEntities _context;

        public UsersRolesRepository(MessengerService_DBEntities context)
        {
            _context = context;
        }

        public async Task<Users> GetUserAsync(string username)
        {
            return await _context.Users.Include(u => u.UserRoles).SingleOrDefaultAsync(a => a.Username == username);
        }

        public Users GetUser(string username)
        {
            return _context.Users.Include(u => u.UserRoles).SingleOrDefault(a => a.Username == username);
        }

        //This method is used to check and validate the user credentials
        public async Task<Users> ValidateUserAsync(string username, string password)
        {
            Users user = await GetUserAsync(username);
            if (user == null) return null;
            //       string[] values = user.HashedPassword.Split(':');
            //       bool valid = Hasher.CheckHash(user.HashedPassword, password);
            bool valid = Hasher.ValidHash(user.HashedPassword, password);
            if (valid) { return user; }
            return null;
        }

        //This method is used to check and validate the user credentials
        public Users ValidateUser(string username, string password)
        {
            Users user = GetUser(username);
            if (user == null) return null;
            //       string[] values = user.HashedPassword.Split(':');
            //       bool valid = Hasher.CheckHash(user.HashedPassword, password);
            bool valid = Hasher.ValidHash(user.HashedPassword, password);
            if (valid) { return user; }
            return null;
        }

        public async Task<bool> AddUserAsync(string username, string password)
        {
            Users user = await GetUserAsync(username);
            if (user != null) { return false; } // Returns false if user exists.
            user = new Users();
            user.Username = username;
            user.HashedPassword = password;
            _context.Users.Add(user);
            return true;
        }

        public async Task<bool> RemoveRolesAsync(string username, List<string> roles)
        {
            Users user = await GetUserAsync(username);
            if (user == null) return false;
            UserRoles userRole;
            foreach (string role in roles)
            {
                userRole = user.UserRoles.FirstOrDefault(a => a.RoleName == role);
                if (userRole != null)
                {
                    _context.UserRoles.Remove(userRole);
                }
            }
            return true;
        }

        public async Task<bool> AddRolesAsync(string username, List<string> roles)
        {
            Users user = await GetUserAsync(username);
            if (user == null) return false;
            UserRoles userRoles;
            foreach (string role in roles)
            {
                userRoles = user.UserRoles.FirstOrDefault(a => a.RoleName == role);
                if (userRoles == null)
                {
                    userRoles = new UserRoles();
                    userRoles.UserID = user.ID;
                    userRoles.RoleName = role;
                    _context.UserRoles.Add(userRoles);
                }
            }
            return true;
        }

        public void RemoveAllRoles(int id)
        {
            var roles = _context.UserRoles.Where(a => a.UserID == id);
            _context.UserRoles.RemoveRange(roles);
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            Users user = await GetUserAsync(username);
            if (user == null) return false;
            _context.Users.Remove(user);
            return true;
        }
    }
}

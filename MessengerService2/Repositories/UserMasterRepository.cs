using MessengerService2.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace MessengerService2.Repositories
{
    public class UserMasterRepository : IUserMasterRepository
    {
        MessengerService_DBEntities context = new MessengerService_DBEntities();

        //This method is used to check and validate the user credentials
        public Users ValidateUser(string username, string password)
        {
            if (IsNullEmptyWhiteSpace(username, password)) return null;
            Users user = null; string[] values; bool valid;
            try
            {
                user = GetUserByName(username);
                values = user.HashedPassword.Split(':');
                valid = Hasher.CheckHash(user.HashedPassword, password);
                if (valid)
                {
                    return user;
                }
            }
            catch { }
            return null;
        }

        public string GetRoles(string username)
        {
            if (IsNullEmptyWhiteSpace(username)) return "";
            string myRoles = ""; int length;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                int i = 0; length = user.UserRoles.Count();
                foreach (var role in user.UserRoles)
                {
                    myRoles += role.RoleName;
                    if ((i + 1) < length)
                    {
                        myRoles += ", ";
                    }
                    i++;
                }
            }
            catch
            {

            }
            return myRoles;
        }

        public bool RemoveRolesFromUser(string username, string[] roles)
        {
            if (IsNullEmptyWhiteSpace(username)) return false;
            UserRoles userRoles;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                foreach (string role in roles)
                {
                    userRoles = context.UserRoles.Single(a => a.UserID == user.ID && a.RoleName == role);
                    if (UserInRole(username, role))
                    {
                        context.UserRoles.Remove(userRoles);
                    }
                }
                context.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public bool AddRolesToUser(string username, string[] roles)
        {
            if (IsNullEmptyWhiteSpace(username)) return false;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                foreach (string role in roles)
                {
                    if (!UserInRole(username, role))
                    {
                        UserRoles userRoles = new UserRoles();
                        userRoles.UserID = user.ID;
                        userRoles.RoleName = role;
                        context.UserRoles.Add(userRoles);
                    }
                }
                context.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public bool RegisterUser(string username, string password, string roles)
        {
            if (IsNullEmptyWhiteSpace(username, password, roles)) return false;
            UserRoles userRoles;
            Users user;
            try
            {
                user = GetUserByName(username);
                if (user != null) // Check if user exists.
                {
                    return false;
                }
                user = new Users();
                user.Username = username;
                user.HashedPassword = password;
                context.Users.Add(user);
                string[] split = roles.Split(',');
                foreach (string s in split)
                {
                    userRoles = new UserRoles();
                    string t = s.Trim();
                    userRoles.RoleName = t;
                    userRoles.UserID = user.ID;
                    context.UserRoles.Add(userRoles);
                }
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool UserInRole(string username, string rolename)
        {
            if (IsNullEmptyWhiteSpace(username, rolename)) return false;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                var roles = from role in context.UserRoles
                            where role.UserID == user.ID && role.RoleName == rolename
                            select role;
                if (roles.Count() == 1)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool DeleteUser(string username)
        {
            if (IsNullEmptyWhiteSpace(username)) return false;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                var roles = from role in context.UserRoles
                            where role.UserID == user.ID
                            select role;
                foreach (var role in roles)
                {
                    context.UserRoles.Remove(role);
                }
                context.Users.Remove(user);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        private Users GetUserByName(string username)
        {
            if (IsNullEmptyWhiteSpace(username)) return null;
            try
            {
                Users user = context.Users.Single(a => a.Username == username);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private bool IsNullEmptyWhiteSpace(string a, string b = "second", string c = "third")
        {
            if ((string.IsNullOrEmpty(a) || string.IsNullOrWhiteSpace(a) || string.IsNullOrEmpty(b) || string.IsNullOrWhiteSpace(b)))
            {
                if ((string.IsNullOrEmpty(c) || string.IsNullOrWhiteSpace(c)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
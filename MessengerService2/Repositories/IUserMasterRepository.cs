using MessengerService2.Models;
using System;

namespace MessengerService2.Repositories
{
    interface IUserMasterRepository : IDisposable
    {
        Users ValidateUser(string username, string password);

        string GetRoles(string username);

        bool RemoveRolesFromUser(string username, string[] roles);

        bool AddRolesToUser(string username, string[] roles);

        bool RegisterUser(string username, string password, string roles);

     //   private bool UserInRole(string username, string rolename);

        bool DeleteUser(string username);

    //    private Users GetUserByName(string username);

    //    void Dispose();
    }
}

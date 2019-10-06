using MessengerService2.Models;
using MessengerService2.Repositories;
using MessengerService2.Services;
using System;
using System.Web;
using System.Web.Http;

namespace MessengerService2.Controllers
{
    public class UsersController : ApiController
    {
        private IUserMasterRepository _userRepo = new UserMasterRepository();

        private IUsersServices _userServices = new UsersServices();

        [HttpPost]
        [AllowAnonymous]
        [Route("api/users/register")]
        public IHttpActionResult Registration(string username, string password, string roles)
        {
            username = HttpUtility.UrlDecode(username);
            password = HttpUtility.UrlDecode(password);
            try
            {
                if (_userRepo.RegisterUser(username, password, roles))
                {
                    return Ok();
                }
            }
            catch { }
            return Conflict();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("api/users/addroles")]
        public IHttpActionResult AddRolesToUser(string username, string roles)
        {
            try
            {
                string[] myRoles = _userServices.stringCSVToArray(roles);
                if(_userRepo.AddRolesToUser(username, myRoles))
                {
                    return Ok();
                }
            }
            catch { }
            return Conflict();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("api/users/removeroles")]
        public IHttpActionResult RemoveRolesFromUser(string username, string roles)
        {
            try
            {
                string[] myRoles = _userServices.stringCSVToArray(roles);
                if (_userRepo.RemoveRolesFromUser(username, myRoles))
                {
                    return Ok();
                }
            }
            catch { }
            return Conflict();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("api/users/delete")]
        public IHttpActionResult DeleteUser(string username)
        {
            if (_userRepo.DeleteUser(username))
            {
                return Ok();
            }
            return Conflict();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

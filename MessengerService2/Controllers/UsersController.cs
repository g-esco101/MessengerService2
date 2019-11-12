using MessengerService2.Helpers;
using MessengerService2.Models;
using MessengerService2.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MessengerService2.Controllers
{
    public class UsersController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public UsersController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/users/register")]
        public async Task<IHttpActionResult> Registration(Dictionary<string, string> userInfo)
        {
            string username = userInfo["username"];
            string password = userInfo["password"];
            List<string> roles = new List<string>();
            roles.Add(userInfo["roles"]);
            if (StringChecker.IsNullEmptyWhiteSpace(username, password)) { return Conflict(); }
            if (StringChecker.IsRoleNullEmptyWhiteSpace(roles)) { return Conflict(); }
            bool userAdded = await _unitOfWork.userRepo.AddUserAsync(username, password);
            if (!userAdded) { return Conflict(); }
            // Must save the user, because Addroles searches user in db. 
            int saved = await _unitOfWork.SaveAsync();
            if (saved < 0) { return Conflict(); }

            bool rolesAdded = await _unitOfWork.userRepo.AddRolesAsync(username, roles);
            if (!rolesAdded) { return Conflict(); }
            saved = await _unitOfWork.SaveAsync();
            if (saved < 0) { return Conflict(); }
            return Ok();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/users/addroles")]
        public async Task<IHttpActionResult> AddRoles(string username, List<string> roles)
        {
            if (StringChecker.IsNullEmptyWhiteSpace(username)) { return Conflict(); }
            if (StringChecker.IsRoleNullEmptyWhiteSpace(roles)) { return Conflict(); }
            bool added = await _unitOfWork.userRepo.AddRolesAsync(username, roles);
            if (added)
            {
                int saved = await _unitOfWork.SaveAsync();
                if (saved < 0) { return Conflict(); }
            }
            else
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("api/users/removeroles")]
        public async Task<IHttpActionResult> RemoveRoles(string username, List<string> roles)
        {
            if (StringChecker.IsNullEmptyWhiteSpace(username)) { return Conflict(); }
            if (StringChecker.IsRoleNullEmptyWhiteSpace(roles)) { return Conflict(); }
            bool rolesRemoved = await _unitOfWork.userRepo.RemoveRolesAsync(username, roles);
            if (!rolesRemoved) { return Conflict(); }
            int saved = await _unitOfWork.SaveAsync();
            if (saved < 0) { return Conflict(); }
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("api/users/delete")]
        public async Task<IHttpActionResult> DeleteUser(string username)
        {
            if (StringChecker.IsNullEmptyWhiteSpace(username)) { return Conflict(); }
            Users user = await _unitOfWork.userRepo.GetUserAsync(username);
            _unitOfWork.userRepo.RemoveAllRoles(user.ID);

            //          IEnumerable<Messages> deleteMsgs = _unitOfWork.msgRepo.Get(username);
            IEnumerable<Messages> deleteMsgs = await _unitOfWork.msgRepo.GetAsync(username);

            _unitOfWork.msgRepo.RemoveRange(deleteMsgs);

            bool userDeleted = await _unitOfWork.userRepo.DeleteUserAsync(username);
            if (!userDeleted) { return Conflict(); }
            int saved = await _unitOfWork.SaveAsync();
            if (saved < 0) { return Conflict(); }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

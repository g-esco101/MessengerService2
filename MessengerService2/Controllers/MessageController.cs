using MessengerService2.Helpers;
using MessengerService2.Models;
using MessengerService2.Unit;
using System.Threading.Tasks;
using System.Web.Http;

namespace MessengerService2.Controllers
{
    public class MessageController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public MessageController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // Stores a message in the database.
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [Route("api/message/send")]
        public async Task<IHttpActionResult> SendMessage(Messages message)
        {
            Users receiver = await _unitOfWork.userRepo.GetUserAsync(message.ReceiverID);
            if (receiver == null) return Conflict();
            message.Queued = true;
            _unitOfWork.msgRepo.Add(message);
            int saved = await _unitOfWork.SaveAsync();
            if (saved < 0) return Conflict();
            return Ok();
        }

        // Returns messages for which the username matches the receiverID.
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("api/message/receive")]
        public async Task<IHttpActionResult> ReceiveMessage(string username)
        {
            if (StringChecker.IsNullEmptyWhiteSpace(username)) { return Conflict(); }
            var messages = await _unitOfWork.msgRepo.GetAsync(username);
            return Ok(messages);
        }

        // Deletes messages for which the username matches the receiverID & the sendername matches the senderID.
        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        [Route("api/message/delete")]
        public async Task<IHttpActionResult> DeleteMessage(string username, string sendername)
        {
            if (StringChecker.IsNullEmptyWhiteSpace(username)) { return Conflict(); }
            Users receiver = await _unitOfWork.userRepo.GetUserAsync(sendername);
            if (receiver == null) return Conflict();
            int deleteCount = await _unitOfWork.msgRepo.DeleteAsync(username, sendername);
            int saved = await _unitOfWork.SaveAsync();
            if (saved < 0) return Conflict();
            return Ok(deleteCount.ToString());
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
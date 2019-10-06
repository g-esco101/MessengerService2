using MessengerService2.Models;
using MessengerService2.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MessengerService2.Controllers
{
    public class MessageController : ApiController
    {
        private IMessageRepository _msgRepo = new MessageRepository();

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [Route("api/message/send")]
        public IHttpActionResult SendMessage(Messages message)
        {
            if (_msgRepo.AddMessage(message))
            {
                return Ok();
            }
            return Conflict();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("api/message/receive")]
        public IHttpActionResult ReceiveMessage(string myId, bool received, bool sent)
        {
            IQueryable<Messages> messages = null;
            messages = _msgRepo.GetMessagesByBool(myId, received, sent);
            if (messages != null)
            {
                List<Messages> myMessages = messages.ToList<Messages>();
                return Ok(myMessages);
            }
            return Conflict();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete]
        [Route("api/message/delete")]
        public IHttpActionResult DeleteMessage(string myId, bool received, bool sent)
        {
            int deleteCount = _msgRepo.DeleteMessages(myId, received, sent);
            if (deleteCount == -1)
            {
                return Conflict();
            }
            return Ok(deleteCount.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _msgRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

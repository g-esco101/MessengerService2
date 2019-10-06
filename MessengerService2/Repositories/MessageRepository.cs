using MessengerService2.Models;
using System.Linq;

namespace MessengerService2.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private MessengerService_DBEntities context = new MessengerService_DBEntities();

        public int DeleteMessages(string myId, bool received, bool sent)
        {
            if (string.IsNullOrEmpty(myId) || string.IsNullOrWhiteSpace(myId)) return -1;
            IQueryable<Messages> messages = GetMessagesByBool(myId, received, sent);
            int deleteCount = 0;
            try
            {
                foreach (Messages msg in messages)
                {
                    context.Messages.Remove(msg);
                    deleteCount++;
                }
                context.SaveChanges();
                return deleteCount;
            }
            catch { return -1; }
        }

        public bool AddMessage(Messages message)
        {
            if (message == null) return false;
            try
            {
                context.Messages.Add(message);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Messages> GetMessagesByBool(string myId, bool received, bool sent)
        {
            if (string.IsNullOrEmpty(myId) || string.IsNullOrWhiteSpace(myId)) return null;
            IQueryable<Messages> messages = null;
            try
            {
                if (received && sent)
                {
                    messages = from msg in context.Messages
                               where msg.SenderID == myId || msg.ReceiverID == myId
                               select msg;
                }
                else if (received)
                {
                    messages = from msg in context.Messages
                               where msg.ReceiverID == myId
                               select msg;
                }
                else if (sent)
                {
                    messages = from msg in context.Messages
                               where msg.SenderID == myId
                               select msg;
                }
            }
            catch { return null; }
            return messages;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
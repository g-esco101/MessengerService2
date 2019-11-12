using MessengerService2.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerService2.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private MessengerService_DBEntities _context;

        public MessagesRepository(MessengerService_DBEntities context)
        {
            _context = context;
        }

        public int Delete(string username, string sendername)
        {
            IEnumerable<Messages> messages = _context.Messages.Where(a => a.ReceiverID == username && a.SenderID == sendername);
            int deleteCount = 0;
            foreach (Messages msg in messages)
            {
                _context.Messages.Remove(msg);
                deleteCount++;
            }
            return deleteCount;
        }

        public async Task<int> DeleteAsync(string username, string sendername)
        {
            var messages = await _context.Messages.Where(a => a.ReceiverID == username && a.SenderID == sendername).ToListAsync();
            int deleteCount = 0;
            foreach (var msg in messages)
            {
                _context.Messages.Remove(msg);
                deleteCount++;
            }
            return deleteCount;
        }

        public void Add(Messages message)
        {
            _context.Messages.Add(message);
        }

        public IEnumerable<Messages> Get(string username)
        {
            return _context.Messages.Where(a => a.ReceiverID == username);
        }

        public async Task<IEnumerable<Messages>> GetAsync(string username)
        {
            return await _context.Messages.Where(a => a.ReceiverID == username).ToListAsync();
        }

        public void RemoveRange(IEnumerable<Messages> entities)
        {
            _context.Messages.RemoveRange(entities);
        }
    }
}
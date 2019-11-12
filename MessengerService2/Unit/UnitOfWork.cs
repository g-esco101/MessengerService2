using MessengerService2.Models;
using MessengerService2.Repositories;
using System.Threading.Tasks;

namespace MessengerService2.Unit
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessengerService_DBEntities _context;

        public UnitOfWork()
        {
            _context = new MessengerService_DBEntities();
            msgRepo = new MessagesRepository(_context);
            userRepo = new UsersRolesRepository(_context);
        }

        public IUsersRolesRepository userRepo { get; private set; }
        public IMessagesRepository msgRepo { get; private set; }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
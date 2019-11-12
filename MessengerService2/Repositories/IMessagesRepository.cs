using MessengerService2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerService2.Repositories
{
    public interface IMessagesRepository
    {
        int Delete(string username, string sender);

        Task<int> DeleteAsync(string username, string sendername);

        void Add(Messages message);

        IEnumerable<Messages> Get(string username);

        Task<IEnumerable<Messages>> GetAsync(string username);

        void RemoveRange(IEnumerable<Messages> entities);
    }
}

using MessengerService2.Models;
using System;
using System.Linq;

namespace MessengerService2.Repositories
{
    interface IMessageRepository : IDisposable
    {
        int DeleteMessages(string myId, bool received, bool sent);

        bool AddMessage(Messages message);

        IQueryable<Messages> GetMessagesByBool(string myId, bool received, bool sent);
    }
}

using MessengerService2.Repositories;
using System;
using System.Threading.Tasks;

namespace MessengerService2.Unit
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRolesRepository userRepo { get; }

        IMessagesRepository msgRepo { get; }


        Task<int> SaveAsync();
    }
}
using MessengerService2.Repositories;
using MessengerService2.Unit;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace MessengerService2
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IMessagesRepository, MessagesRepository>();
            container.RegisterType<IUsersRolesRepository, UsersRolesRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
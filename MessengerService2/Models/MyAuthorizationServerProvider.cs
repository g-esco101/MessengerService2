using MessengerService2.Repositories;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessengerService2.Models
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (MessengerService_DBEntities dbContext = new MessengerService_DBEntities())
            {
                IUsersRolesRepository _repo = new UsersRolesRepository(dbContext);
                var user = _repo.ValidateUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                foreach (var role in user.UserRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                }
                context.Validated(identity);
            }
        }
    }
}
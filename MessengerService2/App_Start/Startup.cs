using MessengerService2.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(MessengerService2.App_Start.Startup))]

namespace MessengerService2.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS (cross origin resource sharing) to make requests using browser from different domains
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                // Path to acquire the token. 
                TokenEndpointPath = new PathString("/token"),
                // Token expiration - day.
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                // Validates user credentials.
                Provider = new MyAuthorizationServerProvider()
            };
            // Adds the authentication middleware to the pipeline. 
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}

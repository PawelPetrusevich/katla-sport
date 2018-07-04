using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KatlaSport.WebApi.App_Start.Startup))]

namespace KatlaSport.WebApi.App_Start
{
    using System;

    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security.OAuth;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions option = new OAuthAuthorizationServerOptions
                                                         {
                                                             TokenEndpointPath = new PathString("/token"),
                                                             Provider = new ApplicationOAuthProvider(),
                                                             AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
                                                             AllowInsecureHttp = true
                                                         };

            app.UseOAuthAuthorizationServer(option);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //app.CreatePerOwinContext<UserContext>(UserContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}
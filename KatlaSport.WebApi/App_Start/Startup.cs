using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KatlaSport.WebApi.App_Start.Startup))]

namespace KatlaSport.WebApi.App_Start
{
    using KatlaSport.DataAccess.Users;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.CreatePerOwinContext<UserContext>(UserContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}
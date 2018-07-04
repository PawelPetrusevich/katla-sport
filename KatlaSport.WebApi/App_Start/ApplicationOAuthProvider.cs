namespace KatlaSport.WebApi.App_Start
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KatlaSport.DataAccess.Users;
    using KatlaSport.DataAccess.Users.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var manager = new UserManager<User>(new UserStore<User>(new UserContext()));
            var user = await manager.FindAsync(context.UserName, context.Password);
            if (user != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("Username", user.UserName));
                identity.AddClaim(new Claim("Email", user.Email));
                identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                context.Validated(identity);
            }
            else
                return;

        }
    }
}
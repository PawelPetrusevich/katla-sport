namespace KatlaSport.DataAccess.Users
{
    using KatlaSport.DataAccess.Users.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(RoleStore<Role> store)
            : base(store)
        {
        }
    }
}
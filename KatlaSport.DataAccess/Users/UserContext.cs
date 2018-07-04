namespace KatlaSport.DataAccess.Users
{
    using System.Data.Entity;

    using KatlaSport.DataAccess.Users.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class UserContext : IdentityDbContext<User>
    {
        public UserContext()
            : base("IdentityDb")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public static UserContext Create()
        {
            return new UserContext();
        }
    }
}
namespace KatlaSport.DataAccess.Users.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        public User()
        {
        }

        public virtual UserProfile UserProfile { get; set; }
    }
}
namespace KatlaSport.DataAccess.Users
{
    using KatlaSport.DataAccess.Users.Models;

    public class ProfileManager : IProfileManager
    {
        public UserContext UserDb { get; set; }

        public ProfileManager(UserContext db)
        {
            UserDb = db;
        }

        public void Dispose()
        {
            UserDb.Dispose();
        }

        public void Create(UserProfile profile)
        {
            UserDb.UserProfiles.Add(profile);
            UserDb.SaveChanges();
        }
    }
}
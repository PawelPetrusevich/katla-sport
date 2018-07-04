namespace KatlaSport.DataAccess.Users
{
    using System;
    using System.Threading.Tasks;

    using KatlaSport.DataAccess.Users.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class UserRepository : IUserRepository
    {
        private UserContext _db;

        private ApplicationUserManager _userManager;

        private ApplicationRoleManager _roleManager;

        private IProfileManager _profileManager;

        public UserRepository()
        {
            _db = new UserContext();
            _userManager = new ApplicationUserManager(new UserStore<User>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<Role>(_db));
            _profileManager = new ProfileManager(_db);
            _userManager.PasswordValidator = CreatePasswordValidator();
        }

        public ApplicationUserManager UserManager => _userManager;

        public IProfileManager ProfileManager => _profileManager;

        public ApplicationRoleManager RoleManager => _roleManager;

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _profileManager.Dispose();
                }

                _disposed = true;
            }
        }

        private PasswordValidator CreatePasswordValidator()
        {
            return new PasswordValidator { RequiredLength = 4, };
        }
    }
}
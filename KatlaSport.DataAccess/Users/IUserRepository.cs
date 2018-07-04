namespace KatlaSport.DataAccess.Users
{
    using System;
    using System.Threading.Tasks;

    public interface IUserRepository : IDisposable
    {
        ApplicationUserManager UserManager { get; }

        IProfileManager ProfileManager { get; }

        ApplicationRoleManager RoleManager { get; }

        Task SaveAsync();
    }
}
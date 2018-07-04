namespace KatlaSport.DataAccess.Users
{
    using System;

    using KatlaSport.DataAccess.Users.Models;

    public interface IProfileManager: IDisposable
    {
        void Create(UserProfile profile);
    }
}
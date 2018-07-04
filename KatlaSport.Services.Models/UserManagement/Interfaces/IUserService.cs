namespace KatlaSport.Services.UserManagement.Interfaces
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KatlaSport.Services.UserManagement.DTO;
    using Microsoft.AspNet.Identity;

    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserRegistrationDto user);

        Task<ClaimsIdentity> Authenticate(UserLoginDto user);

        Task SetInitialData();
    }
}
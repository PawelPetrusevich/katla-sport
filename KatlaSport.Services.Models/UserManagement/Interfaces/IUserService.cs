namespace KatlaSport.Services.UserManagement.Interfaces
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KatlaSport.Services.UserManagement.DTO;

    public interface IUserService
    {
        Task CreateUserAsync(UserRegistrationDto user);

        Task<ClaimsIdentity> Authenticate(UserLoginDto user);

        Task SetInitialData();
    }
}
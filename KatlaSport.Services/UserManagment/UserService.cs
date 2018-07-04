namespace KatlaSport.Services.UserManagment
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using KatlaSport.DataAccess.Users;
    using KatlaSport.DataAccess.Users.Models;
    using KatlaSport.Services.UserManagement.DTO;
    using KatlaSport.Services.UserManagement.Interfaces;

    using Microsoft.AspNet.Identity;

    public class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegistrationDto userRegistrationDto)
        {
            var user = await _repository.UserManager.FindByEmailAsync(userRegistrationDto.Email).ConfigureAwait(false);

            if (user != null)
            {
                throw new ArgumentException();
            }

            user = Mapper.Map<User>(userRegistrationDto);
            var result = await _repository.UserManager.CreateAsync(user, userRegistrationDto.Password).ConfigureAwait(false);
            if (result.Errors.Any())
            {
                throw new ArgumentException("User not created");
            }

            return result;
        }

        public async Task<ClaimsIdentity> Authenticate(UserLoginDto user)
        {
            throw new System.NotImplementedException();
        }

        public async Task SetInitialData()
        {
            throw new System.NotImplementedException();
        }
    }
}
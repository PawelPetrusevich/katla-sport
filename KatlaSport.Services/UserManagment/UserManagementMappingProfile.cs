namespace KatlaSport.Services.UserManagment
{
    using AutoMapper;

    using KatlaSport.DataAccess.Users.Models;
    using KatlaSport.Services.UserManagement.DTO;

    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<User, UserRegistrationDto>();
            CreateMap<UserRegistrationDto, User>().ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Login));
        }
    }
}
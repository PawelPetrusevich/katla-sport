namespace KatlaSport.Services.UserManagement.DTO
{
    using FluentValidation.Attributes;

    using KatlaSport.Services.UserManagement.Validators;

    [Validator(typeof(UserRegistrationDtoValidator))]
    public class UserRegistrationDto
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
namespace KatlaSport.Services.UserManagement.Validators
{
    using FluentValidation;

    using KatlaSport.Services.UserManagement.DTO;

    public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
    {
        public UserRegistrationDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(4);
            RuleFor(x => x.Login).MinimumLength(4);
        }
    }
}
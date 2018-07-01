namespace KatlaSport.Services.HiveManagement.Validator
{
    using FluentValidation;

    using KatlaSport.Services.HiveManagement.DTO;

    /// <summary>
    /// Represents a validator for <see cref="UpdateHiveRequestValidator"/>
    /// </summary>
    public class UpdateHiveRequestValidator : AbstractValidator<UpdateHiveRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateHiveRequestValidator"/> class.
        /// </summary>
        public UpdateHiveRequestValidator()
        {
            RuleFor(r => r.Name).Length(4, 60);
            RuleFor(r => r.Code).Length(5);
            RuleFor(r => r.Address).Length(0, 300);
        }
    }
}

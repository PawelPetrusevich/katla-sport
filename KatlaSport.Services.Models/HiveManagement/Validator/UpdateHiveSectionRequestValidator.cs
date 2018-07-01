namespace KatlaSport.Services.HiveManagement.Validator
{
    using FluentValidation;

    using KatlaSport.Services.HiveManagement.DTO;

    /// <summary>
    /// Represents a validator for <see cref="UpdateHiveSectionRequestValidator"/>
    /// </summary>
    public class UpdateHiveSectionRequestValidator : AbstractValidator<UpdateHiveSectionRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateHiveSectionRequestValidator"/> class.
        /// </summary>
        public UpdateHiveSectionRequestValidator()
        {
            RuleFor(x => x.Code).Length(5);
            RuleFor(x => x.Name).Length(4, 60);
        }
    }
}

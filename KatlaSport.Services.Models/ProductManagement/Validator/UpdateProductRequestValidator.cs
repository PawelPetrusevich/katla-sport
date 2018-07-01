namespace KatlaSport.Services.ProductManagement.Validator
{
    using FluentValidation;

    using KatlaSport.Services.ProductManagement.DTO;

    /// <summary>
    /// Represents a validator for <see cref="UpdateProductRequest"/>.
    /// </summary>
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductRequestValidator"/> class.
        /// </summary>
        public UpdateProductRequestValidator()
        {
            RuleFor(r => r.Name).Length(4, 60);
            RuleFor(r => r.Code).Length(5);
            RuleFor(r => r.CategoryId).GreaterThan(0);

            RuleFor(r => r.Description).MaximumLength(300);
            RuleFor(r => r.ManufacturerCode).MaximumLength(10).NotNull();
            RuleFor(r => r.Price).GreaterThanOrEqualTo(0);
        }
    }
}

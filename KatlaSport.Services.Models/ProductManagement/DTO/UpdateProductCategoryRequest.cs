namespace KatlaSport.Services.ProductManagement.DTO
{
    using FluentValidation.Attributes;

    using KatlaSport.Services.ProductManagement.Validator;

    /// <summary>
    /// Represents a request for creating and updating a product category.
    /// </summary>
    [Validator(typeof(UpdateProductCategoryRequestValidator))]
    public class UpdateProductCategoryRequest
    {
        /// <summary>
        /// Gets or sets a product category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a product category code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a product category description.
        /// </summary>
        public string Description { get; set; }
    }
}

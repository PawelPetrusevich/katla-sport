﻿namespace KatlaSport.Services.ProductManagement.DTO
{
    using System;

    /// <summary>
    /// Represents a product category.
    /// </summary>
    public class ProductCategory
    {
        /// <summary>
        /// Gets or sets a product category identifier.
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether a product category is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a date of the last update.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}

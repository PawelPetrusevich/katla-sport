using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KatlaSport.DataAccess;
using KatlaSport.DataAccess.ProductCatalogue;
using DbProduct = KatlaSport.DataAccess.ProductCatalogue.CatalogueProduct;

namespace KatlaSport.Services.ProductManagement
{
    using KatlaSport.Services.Properties;

    /// <summary>
    /// Represents a product catalogue service.
    /// </summary>
    public class ProductCatalogueService : IProductCatalogueService
    {
        private readonly IProductCatalogueContext _context;
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCatalogueService"/> class with specified <see cref="IProductCatalogueContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="IProductCatalogueContext"/>.</param>
        /// <param name="userContext">A <see cref="IUserContext"/>.</param>
        public ProductCatalogueService(IProductCatalogueContext context, IUserContext userContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        /// <inheritdoc/>
        public async Task<List<ProductListItem>> GetProductsAsync(int start, int amount)
        {
            var dbProducts = await _context.Products.OrderBy(p => p.Id).Skip(start).Take(amount).ToArrayAsync().ConfigureAwait(false);
            var products = dbProducts.Select(p => Mapper.Map<ProductListItem>(p)).ToList();

            return products;
        }

        /// <inheritdoc/>
        public async Task<List<ProductCategoryProductListItem>> GetCategoryProductsAsync(int productCategoryId)
        {
            var dbCategories = await _context.Categories.FirstOrDefaultAsync(p => p.Id == productCategoryId).ConfigureAwait(false);

            if (dbCategories == null)
            {
                throw new RequestedResourceNotFoundException(Resources.ProductNotFound);
            }

            var dbProducts = await _context.Products.OrderBy(p => p.Id).Where(p => p.CategoryId == productCategoryId).ToArrayAsync().ConfigureAwait(false);
            var products = dbProducts.Select(p => Mapper.Map<ProductCategoryProductListItem>(p)).ToList();

            return products;
        }

        /// <inheritdoc/>
        public async Task<Product> GetProductAsync(int productId)
        {
            var dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);

            if (dbProduct == null)
            {
                throw new RequestedResourceNotFoundException(Resources.ProductNotFound);
            }

            return Mapper.Map<DbProduct, Product>(dbProduct);
        }

        /// <inheritdoc/>
        public async Task<Product> CreateProductAsync(UpdateProductRequest createRequest)
        {
            var dbProducts = await _context.Products.FirstOrDefaultAsync(p => p.Code == createRequest.Code).ConfigureAwait(false);
            if (dbProducts != null)
            {
                throw new RequestedResourceHasConflictException(Resources.ProductCodeError);
            }

            var dbProduct = Mapper.Map<UpdateProductRequest, DbProduct>(createRequest);
            dbProduct.CreatedBy = _userContext.UserId;
            dbProduct.LastUpdatedBy = _userContext.UserId;
            _context.Products.Add(dbProduct);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<Product>(dbProduct);
        }

        /// <inheritdoc/>
        public async Task<Product> UpdateProductAsync(int productId, UpdateProductRequest updateRequest)
        {
            var dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Code == updateRequest.Code && p.Id != productId).ConfigureAwait(false);
            if (dbProduct != null)
            {
                throw new RequestedResourceHasConflictException(Resources.ProductCodeError);
            }

            dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);
            if (dbProduct == null)
            {
                throw new RequestedResourceNotFoundException(Resources.ProductNotFound);
            }

            Mapper.Map(updateRequest, dbProduct);
            dbProduct.LastUpdatedBy = _userContext.UserId;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<Product>(dbProduct);
        }

        /// <inheritdoc/>
        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);
            if (product == null)
            {
                throw new RequestedResourceNotFoundException(Resources.ProductNotFound);
            }

            if (product.IsDeleted == false)
            {
                throw new RequestedResourceHasConflictException(Resources.ProductStatusNotDelete);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SetStatusAsync(int productId, bool deletedStatus)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => productId == p.Id).ConfigureAwait(false);

            if (product == null)
            {
                throw new RequestedResourceNotFoundException(Resources.ProductNotFound);
            }

            if (product.IsDeleted != deletedStatus)
            {
                product.IsDeleted = deletedStatus;
                product.LastUpdated = DateTime.UtcNow;
                product.LastUpdatedBy = _userContext.UserId;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}

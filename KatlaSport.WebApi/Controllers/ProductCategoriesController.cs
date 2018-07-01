using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KatlaSport.WebApi.CustomFilters;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace KatlaSport.WebApi.Controllers
{
    using KatlaSport.Services;
    using KatlaSport.Services.ProductManagement.DTO;
    using KatlaSport.Services.ProductManagement.Interfaces;

    [ApiVersion("1.0")]
    [RoutePrefix("api/categories")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class ProductCategoriesController : ApiController
    {
        private readonly IProductCategoryService _categoryService;
        private readonly IProductCatalogueService _productCatalogueService;

        public ProductCategoriesController(IProductCategoryService categoryService, IProductCatalogueService productCatalogueService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _productCatalogueService = productCatalogueService ?? throw new ArgumentNullException(nameof(productCatalogueService));
        }

        /// <summary>
        /// The get product categories.
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of product categories.", Type = typeof(ProductCategoryListItem[]))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Start or amount value is invalid.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetProductCategories([FromUri] int start = 0, [FromUri] int amount = 100)
        {
            if (start < 0)
            {
                return BadRequest("start");
            }

            if (amount < 0)
            {
                return BadRequest("end");
            }

            try
            {
                var categories = await _categoryService.GetCategoriesAsync(start, amount);
                return Ok(categories);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The get product category.
        /// </summary>
        /// <param name="id">
        /// Product catygory ID.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a product category.", Type = typeof(ProductCategory))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product cotegory with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetProductCategory([FromUri] int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryAsync(id);
                return Ok(category);
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The add product category.
        /// </summary>
        /// <param name="createRequest">
        /// Created product category model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("addProductCategory")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "Creates a new product category.", Type = typeof(ProductCategory))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Model is invalid", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Product cotegory with this code exists.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> AddProductCategory([FromBody] UpdateProductCategoryRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryService.CreateCategoryAsync(createRequest);
                var location = string.Format("/api/categories/{0}", category.Id);
                return Created<ProductCategory>(location, category);
            }
            catch (RequestedResourceHasConflictException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The update product category.
        /// </summary>
        /// <param name="id">
        /// Product category ID.
        /// </param>
        /// <param name="updateRequest">
        /// Update model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("update/{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Updates an existed product category.", Type = typeof(ProductCategory))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Model is invalid or product cotegory with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Product cotegory with this code exists.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> UpdateProductCategory([FromUri] int id, [FromBody] UpdateProductCategoryRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _categoryService.UpdateCategoryAsync(id, updateRequest);
                return this.Ok(result);
            }
            catch (RequestedResourceHasConflictException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        // TODO delete cotegory with product?

        /// <summary>
        /// The delete product category.
        /// </summary>
        /// <param name="id">
        /// Product category ID.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        [Route("delete/{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Deletes an existed product category.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product category with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Deleted status not true", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> DeleteProductCategory([FromUri] int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (RequestedResourceHasConflictException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The set delete status.
        /// </summary>
        /// <param name="id">
        /// Product category ID.
        /// </param>
        /// <param name="deletedStatus">
        /// Boolean deleted status.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("{id:int:min(1)}/status/{deletedStatus:bool}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Sets deleted status for an existed product category.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product category with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> SetStatus([FromUri] int id, [FromUri] bool deletedStatus)
        {
            try
            {
                await _categoryService.SetStatusAsync(id, deletedStatus);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The get products by product category ID.
        /// </summary>
        /// <param name="id">
        /// Product category ID.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("{id:int:min(1)}/products")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of products for requested product category.", Type = typeof(ProductCategoryProductListItem))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product category with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetProducts([FromUri] int id)
        {
            try
            {
                var products = await _productCatalogueService.GetCategoryProductsAsync(id);
                return Ok(products);
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KatlaSport.Services.ProductManagement;
using KatlaSport.WebApi.CustomFilters;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace KatlaSport.WebApi.Controllers
{
    using KatlaSport.Services;

    // todo routes
    [ApiVersion("1.0")]
    [RoutePrefix("api/products")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class ProductsController : ApiController
    {
        private readonly IProductCatalogueService _productService;

        public ProductsController(IProductCatalogueService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// The get products.
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
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of products.", Type = typeof(ProductListItem[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetProducts([FromUri] int start = 0, [FromUri] int amount = 100)
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
                var products = await _productService.GetProductsAsync(start, amount);
                return Ok(products);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// The get product by product id.
        /// </summary>
        /// <param name="id">
        /// Product ID.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a product.", Type = typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetProduct([FromUri] int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                return Ok(product);
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
        /// Create product.
        /// </summary>
        /// <param name="createRequest">
        /// Created product model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "Creates a new product.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Model is invalid", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Product with this code exists", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> AddProduct([FromBody] UpdateProductRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.CreateProductAsync(createRequest);
                var location = string.Format("/api/products/{0}", product.Id);
                return Created<Product>(location, product);
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
        /// The update product.
        /// </summary>
        /// <param name="id">
        /// Product ID.
        /// </param>
        /// <param name="updateRequest">
        /// Update model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Updates an existed product.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product with this ID not found or model is invalid", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Product with this code exists", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] int id, [FromBody] UpdateProductRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // todo return value
                await _productService.UpdateProductAsync(id, updateRequest);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (RequestedResourceHasConflictException ex)
            {
                return this.ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// Delete product.
        /// </summary>
        /// <param name="id">
        /// Product ID.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Deletes an existed product.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Deleted status nit true", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
            }
            catch (RequestedResourceNotFoundException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (RequestedResourceHasConflictException ex)
            {
                return this.ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// Set deleted status.
        /// </summary>
        /// <param name="id">
        /// Product ID.
        /// </param>
        /// <param name="deletedStatus">
        /// Bollean deleted status.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("{id:int:min(1)}/status/{deletedStatus:bool}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Sets deleted status for an existed product category.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Product with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> SetStatus([FromUri] int id, [FromUri] bool deletedStatus)
        {
            try
            {
                await _productService.SetStatusAsync(id, deletedStatus);
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
    }
}

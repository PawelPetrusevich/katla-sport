namespace KatlaSport.WebApi.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using KatlaSport.Services;
    using KatlaSport.Services.HiveManagement;
    using KatlaSport.WebApi.CustomFilters;
    using KatlaSport.WebApi.Properties;

    using Microsoft.Web.Http;

    using Swashbuckle.Swagger.Annotations;

    [ApiVersion("1.0")]
    [RoutePrefix("api/hives")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class HivesController : ApiController
    {
        private readonly IHiveService _hiveService;
        private readonly IHiveSectionService _hiveSectionService;

        public HivesController(IHiveService hiveService, IHiveSectionService hiveSectionService)
        {
            _hiveService = hiveService ?? throw new ArgumentNullException(nameof(hiveService));
            _hiveSectionService = hiveSectionService ?? throw new ArgumentNullException(nameof(hiveSectionService));
        }

        /// <summary>
        /// Get hives async
        /// </summary>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of hives.", Type = typeof(HiveListItem[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetHivesAsync()
        {
            try
            {
                var hives = await _hiveService.GetHivesAsync();
                return this.Ok(hives);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// Get hive async by id.
        /// </summary>
        /// <param name="hiveId">
        /// Hive id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpGet]
        [Route("{hiveId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a hive.", Type = typeof(Hive))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetHiveAsync(int hiveId)
        {
            try
            {
                var hive = await _hiveService.GetHiveAsync(hiveId);
                return Ok(hive);
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
        /// Get hive sections async by hive id.
        /// </summary>
        /// <param name="hiveId">
        /// The hive id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpGet]
        [Route("{hiveId:int:min(1)}/sections")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of hive sections for specified hive.", Type = typeof(HiveSectionListItem))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server Error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetHiveSectionsAsync(int hiveId)
        {
            try
            {
                var hive = await _hiveSectionService.GetHiveSectionsAsync(hiveId);
                return Ok(hive);
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
        /// Set hive deleted status.
        /// </summary>
        /// <param name="hiveId">
        /// The hive id.
        /// </param>
        /// <param name="deletedStatus">
        /// Bool deleted status.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPut]
        [Route("{hiveId:int:min(1)}/status/{deletedStatus:bool}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Sets deleted status for an existed hive.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> SetStatusAsync([FromUri] int hiveId, [FromUri] bool deletedStatus)
        {
            try
            {
                await _hiveService.SetStatusAsync(hiveId, deletedStatus);
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
        /// Create hive.
        /// </summary>
        /// <param name="hive">
        /// Update hive request.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPost]
        [Route("addHive")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "Return created hive", Type = typeof(Hive))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive model value is invalid or null", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Hive exists with this code", Type = typeof(string))]
        public async Task<IHttpActionResult> CreateHiveAsync([FromBody] UpdateHiveRequest hive)
        {
            if (hive == null)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            try
            {
                var result = await this._hiveService.CreateHiveAsync(hive);
                var location = $"api/hives/{result.Id}";
                return this.Created<Hive>(location, result);
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
        /// Update hive.
        /// </summary>
        /// <param name="id">
        /// Hive id.
        /// </param>
        /// <param name="hive">
        /// Update hive request model.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPut]
        [Route("update/{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Updates an existed hive.",Type = typeof(Hive))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive model value is invalid or null", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Hive exists with this code", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Hive exists with this ID", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> UpdateHiveAsync([FromUri] int id, [FromBody] UpdateHiveRequest hive)
        {
            if (hive == null)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            try
            {
                var result = await this._hiveService.UpdateHiveAsync(id, hive);
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

        /// <summary>
        /// Delete hive.
        /// </summary>
        /// <param name="id">
        /// Hive id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Deletes an existed hive.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Deleted status not true", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> DeleteHiveASync([FromUri] int id)
        {
            try
            {
                await this._hiveService.DeleteHiveAsync(id);
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
    }
}

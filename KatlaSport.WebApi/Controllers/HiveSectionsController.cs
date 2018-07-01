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
    using KatlaSport.Services.HiveManagement.DTO;
    using KatlaSport.Services.HiveManagement.Interfaces;
    using KatlaSport.WebApi.Properties;

    [ApiVersion("1.0")]
    [RoutePrefix("api/sections")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class HiveSectionsController : ApiController
    {
        private readonly IHiveSectionService _hiveSectionService;

        public HiveSectionsController(IHiveSectionService hiveSectionService)
        {
            _hiveSectionService = hiveSectionService ?? throw new ArgumentNullException(nameof(hiveSectionService));
        }

        /// <summary>
        /// Get hive sections.
        /// </summary>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of hive sections.", Type = typeof(HiveSectionListItem[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> GetHiveSections()
        {
            try
            {
                var hives = await _hiveSectionService.GetHiveSectionsAsync();
                return Ok(hives);
            }
            catch (Exception)
            {
                return this.InternalServerError();
            }
        }

        /// <summary>
        /// Get hive section.
        /// </summary>
        /// <param name="hiveSectionId">
        /// Hive section id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpGet]
        [Route("{hiveSectionId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a hive section.", Type = typeof(HiveSection))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive section with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetHiveSection(int hiveSectionId)
        {
            try
            {
                var hive = await _hiveSectionService.GetHiveSectionAsync(hiveSectionId);
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
        /// Set delete status for hive section.
        /// </summary>
        /// <param name="hiveSectionId">
        /// Hive section id.
        /// </param>
        /// <param name="deletedStatus">
        /// Bool deleted status.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPut]
        [Route("{hiveSectionId:int:min(1)}/status/{deletedStatus:bool}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Sets deleted status for an existed hive section.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive section with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> SetStatus([FromUri] int hiveSectionId, [FromUri] bool deletedStatus)
        {
            try
            {
                await _hiveSectionService.SetStatusAsync(hiveSectionId, deletedStatus);
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
        /// Create hive section.
        /// </summary>
        /// <param name="hiveSection">
        /// The hive section.
        /// </param>
        /// <param name="hiveId">
        /// The hive id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPost]
        [Route("addHiveSection/{hiveId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "Return created hive section", Type = typeof(HiveSection))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive with this ID not found or hive section model is invalid", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Hive section exists with this code", Type = typeof(string))]
        public async Task<IHttpActionResult> CreateHiveSection([FromBody] UpdateHiveSectionRequest hiveSection, [FromUri] int hiveId)
        {
            if (hiveSection == null)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            try
            {
                var result = await this._hiveSectionService.CreateHiveSectionAsync(hiveSection, hiveId);
                var location = $"api/sections/{result.Id}";
                return this.Created<HiveSection>(location, result);
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
        /// Update hive section.
        /// </summary>
        /// <param name="id">
        /// The hive section id.
        /// </param>
        /// <param name="hiveSection">
        /// The hive section model.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpPut]
        [Route("update/{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Updates an existed hive section.", Type = typeof(HiveSection))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive section with this ID not found or hive section model is invalid.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Hive section exists with this code.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> UpdateHiveSection([FromUri] int id, [FromBody] UpdateHiveSectionRequest hiveSection)
        {
            if (hiveSection == null)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(Resources.ModelIsInvalid);
            }

            try
            {
                var result = await this._hiveSectionService.UpdateHiveSectionAsync(id, hiveSection);
                return this.Ok(result);
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
        /// Delete hive section.
        /// </summary>
        /// <param name="id">
        /// Hive section id.
        /// </param>
        /// <returns>
        /// The <see cref="Task{IHttpActionResult}"/>.
        /// </returns>
        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Deletes an existed hive section.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Hive section with this ID not found", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Hive section has not IsDeleted status true.", Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Server error", Type = typeof(string))]
        public async Task<IHttpActionResult> DeleteHiveSection([FromUri] int id)
        {
            try
            {
                await this._hiveSectionService.DeleteHiveSectionAsync(id);
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

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using KatlaSport.Services.HiveManagement;
using KatlaSport.WebApi.CustomFilters;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace KatlaSport.WebApi.Controllers
{
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

        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of hive sections.", Type = typeof(HiveSectionListItem[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetHiveSections()
        {
            var hives = await _hiveSectionService.GetHiveSectionsAsync();
            return Ok(hives);
        }

        [HttpGet]
        [Route("{hiveSectionId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a hive section.", Type = typeof(HiveSection))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetHiveSection(int hiveSectionId)
        {
            var hive = await _hiveSectionService.GetHiveSectionAsync(hiveSectionId);
            return Ok(hive);
        }

        [HttpPut]
        [Route("{hiveSectionId:int:min(1)}/status/{deletedStatus:bool}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Sets deleted status for an existed hive section.")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> SetStatus([FromUri] int hiveSectionId, [FromUri] bool deletedStatus)
        {
            await _hiveSectionService.SetStatusAsync(hiveSectionId, deletedStatus);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [HttpPost]
        [Route("addHiveSection")]
        [SwaggerResponse(HttpStatusCode.Created, Description = "Return created hive section", Type = typeof(HiveSection))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
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

            var result = await this._hiveSectionService.CreateHiveSection(hiveSection, hiveId);
            var location = $"api/sections/{result.Id}";
            return this.Created<HiveSection>(location, result);
        }

        [HttpPut]
        [Route("update/{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Updates an existed hive section.")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
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

            var result = await this._hiveSectionService.UpdateHiveSection(id, hiveSection);

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent, Description = "Deletes an existed hive section.")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> DeleteHive([FromUri] int id)
        {
            await this._hiveSectionService.DeleteHiveSection(id);

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}

namespace KatlaSport.WebApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using KatlaSport.Services.UserManagement.DTO;
    using KatlaSport.Services.UserManagement.Interfaces;
    using KatlaSport.WebApi.CustomFilters;

    using Microsoft.Web.Http;

    using Swashbuckle.Swagger.Annotations;

    [ApiVersion("1.0")]
    [RoutePrefix("api/users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IHttpActionResult> Registration([FromBody] UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            try
            {
                await this._userService.CreateUserAsync(userRegistrationDto);
                return this.Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

    }
}
namespace KatlaSport.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
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
                var result = await this._userService.CreateUserAsync(userRegistrationDto);
                return this.Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetUserClaims")]
        public AccountModel GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            AccountModel model = new AccountModel()
                                     {
                                         UserName = identityClaims.FindFirst("Username").Value,
                                         Email = identityClaims.FindFirst("Email").Value,
                                         LoggedOn = identityClaims.FindFirst("LoggedOn").Value
                                     };
            return model;
        }

    }

    public class AccountModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string LoggedOn { get; set; }
    }
}
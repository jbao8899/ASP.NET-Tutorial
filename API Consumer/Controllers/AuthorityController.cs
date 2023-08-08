using API_Consumer.Authority;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Consumer.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        IConfiguration _configuration;

        public AuthorityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AppCredential credential)
        {
            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                DateTime expires = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    AccessToken = Authenticator.CreateToken(credential.ClientId, expires, _configuration.GetValue<string>("SecretKey")),
                    Expires = expires
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized");
                ValidationProblemDetails problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);

            }
        }
    }
}

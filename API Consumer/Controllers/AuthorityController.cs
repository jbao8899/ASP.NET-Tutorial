using API_Consumer.Authority;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Consumer.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AppCredential credential)
        {
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                return Ok(new
                {
                    AccessToken = CreateToken(credential.ClientId),
                    Expires = DateTime.UtcNow.AddMinutes(10)
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

        private string CreateToken(string clientId)
        {
            return "";
        }
    }
}

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
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                DateTime expires = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    AccessToken = CreateToken(credential.ClientId, expires),
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

        private string CreateToken(string clientId, DateTime expiresAt)
        {
            // Key
            byte[]? secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));

            Application? app = AppRepository.GetApplication(clientId);

            // Claims
            List<Claim> claims = new List<Claim>()
            {
                new Claim("AppName", app?.ApplicationName??string.Empty),
                new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false")
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}

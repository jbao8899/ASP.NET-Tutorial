using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Authority;

namespace Server.Filters.AuthFilters
{
    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            if (!Authenticator.VerifyToken(token!, configuration.GetValue<String>("SecretKey")))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}

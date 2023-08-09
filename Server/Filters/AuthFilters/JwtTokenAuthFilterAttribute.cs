using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Attributes;
using Server.Authority;
using System.Security.Claims;

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

            IEnumerable<Claim>? claims = Authenticator.VerifyToken(token!, configuration.GetValue<String>("SecretKey"));
            if (claims is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                var requiredClaims = context.ActionDescriptor
                                            .EndpointMetadata
                                            .OfType<RequiredClaimAttribute>()
                                            .ToList();

                if (requiredClaims is not null &&
                    !requiredClaims.All(rc => 
                        claims.Any(c => c.Type.Trim().ToLower() == rc.ClaimType.Trim().ToLower() &&
                            c.Value.Trim().ToLower() == rc.ClaimValue.Trim().ToLower())))
                {
                    // at least one required claim not satisfied
                    context.Result = new StatusCodeResult(403);
                    return;
                }

                return;
            }
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace WebApplication1.Filters.AuthFilters
//{
//    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
//    {
//        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            Authenticator.VerifyToken
//        }
//    }
//}

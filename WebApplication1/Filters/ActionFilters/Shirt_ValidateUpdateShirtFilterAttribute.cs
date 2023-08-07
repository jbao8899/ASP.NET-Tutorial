using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Models;

namespace WebApplication1.Filters.ActionFilters
{
    public class Shirt_ValidateUpdateShirtFilterAttribute : ActionFilterAttribute
    {
        // doesn't contact the database
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            int? id = (int?)context.ActionArguments["id"];
            Shirt? shirt = (Shirt?)context.ActionArguments["shirt"];

            if (shirt != null &&
                id != null &&
                shirt.Id != id)
            {
                context.ModelState.AddModelError("Id", "shirt.Id does not equal id");

                ValidationProblemDetails problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);

            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Filters.ActionFilters
{
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _context;

        public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            base.OnActionExecuting(actionExecutingContext);

            int? shirtId = actionExecutingContext.ActionArguments["id"] as int?;

            if (shirtId != null)
            {
                if (shirtId <= 0)
                {
                    actionExecutingContext.ModelState.AddModelError("Id", "Id should be positive");

                    ValidationProblemDetails problemDetails = new ValidationProblemDetails(actionExecutingContext.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    actionExecutingContext.Result = new BadRequestObjectResult(problemDetails);
                }
                else // if (!ShirtRepository.ShirtExists((int)shirtId))
                {
                    Shirt? shirt = _context.Shirts.Find(shirtId);

                    if (shirt == null)
                    {
                        actionExecutingContext.ModelState.AddModelError("Id", "Shirt with this id does not exist");

                        ValidationProblemDetails problemDetails = new ValidationProblemDetails(actionExecutingContext.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };

                        actionExecutingContext.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        // found the shirt
                        actionExecutingContext.HttpContext.Items["shirt"] = shirt; // pass it to the controller
                    }
                }
            }
            else
            {
                // No else case????
            }

        }
    }
}

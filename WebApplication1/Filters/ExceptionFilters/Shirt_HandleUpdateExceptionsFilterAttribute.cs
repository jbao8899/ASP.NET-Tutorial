using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Filters.ExceptionFilters
{
    public class Shirt_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext _context;

        public Shirt_HandleUpdateExceptionsFilterAttribute(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void OnException(ExceptionContext exceptionContext)
        {
            base.OnException(exceptionContext);

            string? strStringId = (string?)exceptionContext.RouteData.Values["id"];

            // check for null strStringId???

            if (!int.TryParse(strStringId, out int shirtId))
            {
                Shirt toUpdate = _context.Shirts.FirstOrDefault(s => s.Id == shirtId)!;
                if (toUpdate == null)
                //if (!ShirtRepository.ShirtExists(shirtId))
                {
                    exceptionContext.ModelState.AddModelError("ShirtId", $"The shirt with an id of {shirtId} does not exist");
                    ValidationProblemDetails problemDetails = new ValidationProblemDetails(exceptionContext.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    exceptionContext.Result = new BadRequestObjectResult(problemDetails);

                }
                else
                {
                    // found the shirt
                    exceptionContext.HttpContext.Items["shirt"] = toUpdate; // pass it to the controller
                }
            }
        }
    }
}

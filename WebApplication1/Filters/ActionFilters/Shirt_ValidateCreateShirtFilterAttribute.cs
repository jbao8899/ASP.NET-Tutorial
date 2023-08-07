using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _context;

        public Shirt_ValidateCreateShirtFilterAttribute(ApplicationDbContext context)
        {
            _context = context;
        }

        //Shirt? searchedShirt = (
        //    from shirt in _shirts
        //    where shirt.BrandName == query.BrandName
        //    where shirt.Color == query.Color
        //    where shirt.Size == query.Size
        //    where shirt.IsForMen == query.IsForMen
        //    where shirt.Price == query.Price
        //    select shirt
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            base.OnActionExecuting(actionExecutingContext);

            Shirt? query = (Shirt?)actionExecutingContext.ActionArguments["shirt"];

            if (query == null)
            {
                actionExecutingContext.ModelState.AddModelError("Shirt", "Shirt object is null");
                ValidationProblemDetails problemDetails = new ValidationProblemDetails(actionExecutingContext.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                actionExecutingContext.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                // Shirt is not null, investigate if it equals any existing shirt.
                //Shirt? existingShirt = ShirtRepository.GetShirtByProperties(query);
                Shirt? existingShirt = (from shirtInLinq in _context.Shirts
                                        where shirtInLinq.BrandName.ToLower() == query.BrandName.ToLower()
                                        where shirtInLinq.Color.ToLower() == query.Color.ToLower()
                                        where shirtInLinq.Size == query.Size
                                        where shirtInLinq.IsForMen == query.IsForMen
                                        where shirtInLinq.Price == query.Price
                                        select shirtInLinq).FirstOrDefault();

                if (existingShirt != null)
                {
                    actionExecutingContext.ModelState.AddModelError("Shirt", "Shirt already exists");
                    ValidationProblemDetails problemDetails = new ValidationProblemDetails(actionExecutingContext.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    actionExecutingContext.Result = new BadRequestObjectResult(problemDetails);

                }
            }
        }
    }
}

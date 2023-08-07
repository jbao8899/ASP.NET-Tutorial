using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Filters.ActionFilters;
using WebApplication1.Filters.ExceptionFilters;
using WebApplication1.Models;
using WebApplication1.Models.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class ShirtControllers : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShirtControllers(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/shirts")]
        public IActionResult GetShirts()
        {
            //return Ok(ShirtRepository.GetShirts());

            return Ok(_context.Shirts.ToList());
        }

        [HttpGet]
        [Route("/shirts/{id}")]
        // [Shirt_ValidateShirtIdFilter] // Can't use this after using dependency injection
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            // Internal error checking
            //if (id < 0)
            //{
            //    return BadRequest(new { Message = $"id must be positive" });
            //}
            //Shirt? shirt = ShirtRepository.GetShirtById(id);
            //if (shirt != null)
            //{
            //    return Ok(shirt);
            //}
            //else
            //{
            //    return NotFound(new { Message = $"No contact with an ID of {id} was found." });
            //}

            // We use a filter for error checking, so we don't need it here
            //return Ok(ShirtRepository.GetShirtById(id));

            // We get the shirt in the action filter, so we do not want to get it again 
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [Route("/shirts")]
        //[Shirt_ValidateCreateShirtFilter] // Can't use this after using dependency injection
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            // Not needed due to filter
            //if (shirt == null)
            //{
            //    return BadRequest();
            //}

            //if (ShirtRepository.GetShirtByProperties(shirt) != null)
            //{
            //    return BadRequest();
            //}

            //ShirtRepository.AddShirt(shirt);

            _context.Shirts.Add(shirt);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetShirtById),
                                   new { Id = shirt.Id },
                                   shirt);
        }

        //[HttpPost]
        //[Route("/shirts")]
        //public string CreateShirt()
        //{
        //    return "Creating a shirt";
        //}

        //[HttpPost]
        //[Route("/shirts/{color}")]
        //public string CreateShirtWithColorFromRoute([FromRoute] string color)
        //{
        //    return $"Creating a {color} shirt";
        //}

        //[HttpPost]
        //[Route("/shirtsQuery")]
        //public string CreateShirtWithColorFromQuery([FromQuery] string color)
        //{
        //    // https://localhost:7283/shirtsQuery?color=red
        //    return $"Creating a {color} shirt";
        //}

        //[HttpPost]
        //[Route("/shirtsHeader")]
        //public string CreateShirtWithColorFromHeader([FromHeader(Name = "color")] string color)
        //{
        //    // https://localhost:7283/shirtsHeader?color=red
        //    return $"Creating a {color} shirt";
        //}

        [HttpPut]
        [Route("/shirts/{id}")]
        // [Shirt_ValidateShirtIdFilter] // Can't use this after using dependency injection
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        //[Shirt_HandleUpdateExceptionsFilter]  // Can't use this after using dependency injection
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
        public IActionResult UpdateShirt(int id, [FromBody]Shirt shirt)
        {
            //if (id != shirt.Id)
            //{
            //    return BadRequest();
            //}

            // Exception filter makes this redundant
            //try
            //{
            //    // Shirt with this id may not exist
            //    ShirtRepository.UpdateShirt(shirt);
            //}
            //catch // Don't care about exception type
            //{
            //    if (!ShirtRepository.ShirtExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw; // generic error
            //    }
            //}


            //ShirtRepository.UpdateShirt(shirt);
            Shirt toUpdate = (Shirt)HttpContext.Items["shirt"]!;
            toUpdate.BrandName = shirt.BrandName;
            toUpdate.Color = shirt.Color;
            toUpdate.Size = shirt.Size;
            toUpdate.IsForMen = shirt.IsForMen;
            toUpdate.Price = shirt.Price;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("/shirts/{id}")]
        // [Shirt_ValidateShirtIdFilter] // Can't use this after using dependency injection
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            // We delete by id, but retrieve the shirt to be deleted, so that
            // it can be returned
            //Shirt? toDelete = ShirtRepository.GetShirtById(id);
            //ShirtRepository.DeleteShirt(toDelete.Id);

            _context.Shirts.Remove((Shirt)HttpContext.Items["shirt"]!);
            _context.SaveChanges();

            return Ok(HttpContext.Items["shirt"]);
        }
    }
}

using API_Consumer.Data;
using API_Consumer.Models;
using API_Consumer.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Consumer.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecutor _webApiExecutor;

        public ShirtsController(IWebApiExecutor webApiExecutor)
        {
            _webApiExecutor = webApiExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _webApiExecutor.InvokeGet<List<Shirt>>("shirts"));
        }

        public IActionResult CreateShirt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                var response = _webApiExecutor.InvokePost<Shirt>("shirts", shirt);
                if (response is not null)
                {
                    // go back to showing all shirts
                    return RedirectToAction(nameof(Index));
                }
            }
            // Show the incorrect shirt
            return View(shirt);
        }

        //public IActionResult UpdateShirt()
        //{
        //    return View();
        //}

        //[HttpPut] // doesn't work if this is included
        //[HttpPost] // doesn't work either
        // This is only for displaying stuff, so it should not be annotated
        // with an HTTP request type
        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            Shirt? shirt = await _webApiExecutor.InvokeGet<Shirt>($"shirts/{shirtId}");
            if (shirt is not null)
            {
                return View(shirt);
            }

            return NotFound();
        }

        [HttpPost] // use post even though we are updating.
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                await _webApiExecutor.InvokePut<Shirt>($"shirts/{shirt.Id}", shirt);
                // go back to showing all shirts
                return RedirectToAction(nameof(Index));

            }
            // Show the incorrect shirt
            return View(shirt);
        }

        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            await _webApiExecutor.InvokeDelete<Shirt>($"shirts/{shirtId}");
            return RedirectToAction(nameof(Index));

        }
        
    }
}

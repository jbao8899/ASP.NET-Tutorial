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
            return View(shirt);
        }
    }
}

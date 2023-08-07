using API_Consumer.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Consumer.Controllers
{
    public class ShirtsController : Controller
    {
        public IActionResult Index()
        {
            return View(ShirtRepository.GetShirts());
        }
    }
}

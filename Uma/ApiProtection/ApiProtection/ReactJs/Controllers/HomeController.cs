using Microsoft.AspNetCore.Mvc;

namespace ApiProtection.ReactJs.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}

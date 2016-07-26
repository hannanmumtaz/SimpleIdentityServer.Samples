using Microsoft.AspNetCore.Mvc;

namespace Scenario2.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;

namespace ApiProtection.ReactJs.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public HomeController(IIdentityServerClientFactory identityServerClientFactory)
        {
            _identityServerClientFactory = identityServerClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}

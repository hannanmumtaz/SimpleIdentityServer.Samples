using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using System.Threading.Tasks;

namespace MedicalWebsite.Controllers
{
    public class BaseViewModel : Controller
    {
        public async Task GetViewModel()
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            var result = await identityServerClientFactory.CreateDiscoveryClient()
                .GetDiscoveryInformationAsync(Constants.OpenIdConfigurationUrl);
            ViewData["AuthorizationUrl"] = result.AuthorizationEndPoint;
        }
    }
}

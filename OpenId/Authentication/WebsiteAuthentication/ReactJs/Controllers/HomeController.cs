using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using System;
using System.Threading.Tasks;
using WebsiteAuthentication.ReactJs.DTOs;

namespace WebsiteAuthentication.ReactJs.Controllers
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

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest authenticateRequest)
        {
            Check(authenticateRequest);
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretPostAuth(Constants.ClientId, Constants.ClientSecret)
                .UsePassword(authenticateRequest.Login, authenticateRequest.Password, "openid", "profile", "role")
                .ResolveAsync(Constants.OpenIdWellKnownConfiguration)
                .ConfigureAwait(false);
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(grantedToken);
        }

        private static void Check(AuthenticateRequest authenticateRequest)
        {
            if (authenticateRequest == null)
            {
                throw new ArgumentNullException(nameof(authenticateRequest));
            }

            if (string.IsNullOrWhiteSpace(authenticateRequest.Login))
            {
                throw new ArgumentNullException(nameof(authenticateRequest.Login));
            }

            if (string.IsNullOrWhiteSpace(authenticateRequest.Password))
            {
                throw new ArgumentNullException(nameof(authenticateRequest.Password));
            }
        }
    }
}

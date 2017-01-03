using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.Uma.Common;
using SimpleIdentityServer.Uma.Common.DTOs;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Extensions;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IJwsParser _jwsParser;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public AuthenticateController(IJwsParser jwsParser)
        {
            _jwsParser = jwsParser;
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();
            _identityServerClientFactory = new IdentityServerClientFactory();
        }

        public async Task<IActionResult> Index()
        {
            var discovery = await _identityServerClientFactory.CreateDiscoveryClient()
                .GetDiscoveryInformationAsync(Constants.OpenIdConfigurationUrl);
            return View(new AuthenticateViewModel
            {
                AuthorizationEndPoint = discovery.AuthorizationEndPoint
            });
        }

        public async Task<IActionResult> Callback()
        {
            var idToken = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "id_token");
            var accessToken = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "access_token");
            if (idToken.Equals(default(KeyValuePair<string, StringValues>)))
            {
                return null;
            }

            if (accessToken.Equals(default(KeyValuePair<string, StringValues>)))
            {
                return null;
            }

            // Add claims
            var jwsPayload = _jwsParser.GetPayload(idToken.Value);
            var claims = new List<Claim>();
            foreach (var claim in jwsPayload)
            {
                claims.Add(new Claim(claim.Key, claim.Value.ToString()));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Add permissions
            var introspectionClient = _identityServerUmaClientFactory.GetIntrospectionClient();
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            var rpts = await SecurityProxyWebApplication.GetRptTokenByRecursion(idToken.Value, 
                accessToken.Value.First(), 
                accessToken.Value.First(),
                accessToken.Value.First());
            var introspections = await introspectionClient.GetByResolution(new PostIntrospection
            {
                Rpts = rpts
            }, Constants.UmaConfigurationUrl);
            foreach(var introspection in introspections)
            {
                foreach(var permission in introspection.Permissions)
                {
                    var claimPermission = new Permission
                    {
                        ResourceSetId = permission.ResourceSetId,
                        Scopes = permission.Scopes
                    };

                    var operations = await resourceClient
                        .SearchResources(new SearchResourceRequest
                        {
                            ResourceId = permission.ResourceSetId
                        }, Constants.ResourcesUrl, accessToken.Value.First());
                    if (operations != null && operations.Any())
                    {
                        var operation = operations.First();
                        claimPermission.Url = operation.Url;
                    }

                    claimsIdentity.AddPermission(claimPermission);
                }
            }

            // Set claims & create the cookie
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authenticationManager = this.GetAuthenticationManager();
            await authenticationManager.SignInAsync(Constants.CookieWebApplicationName,
                principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    IsPersistent = false
                });
            return RedirectToAction("Index", "Home");
        }
    }
}

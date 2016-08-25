using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Client.DTOs.Responses;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.Uma.Common;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Extensions;

namespace WebApplication.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IJwsParser _jwsParser;

        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;

        private readonly IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory;

        public AuthenticateController(IJwsParser jwsParser)
        {
            _jwsParser = jwsParser;
            _identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Callback()
        {
            StringValues identityToken,
                accessToken;
            if (!Request.Query.TryGetValue("id_token", out identityToken))
            {
                return null;
            }

            if (!Request.Query.TryGetValue("access_token", out accessToken))
            {
                return null;
            }

            // Add claims
            var jwsPayload = _jwsParser.GetPayload(identityToken);
            var claims = new List<Claim>();
            foreach (var claim in jwsPayload)
            {
                claims.Add(new Claim(claim.Key, claim.Value.ToString()));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Add permissions
            var introspectionClient = _identityServerUmaClientFactory.GetIntrospectionClient();
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            List<string> rpts = await SecurityProxyWebApplication.GetRptTokenByRecursion(identityToken.First(), 
                accessToken.First(), 
                accessToken.First());
            foreach (string rpt in rpts)
            {
                IntrospectionResponse introspectionResponse = await introspectionClient.GetIntrospectionByResolvingUrlAsync(rpt, "https://localhost:5445/.well-known/uma-configuration");
                if (introspectionResponse == null ||
                    introspectionResponse.Permissions == null ||
                    !introspectionResponse.Permissions.Any())
                {
                    continue;
                }

                foreach(var permission in introspectionResponse.Permissions)
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
                        }, Constants.ResourcesUrl, string.Empty);
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
            authenticationManager.SignInAsync(Constants.CookieWebApplicationName,
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

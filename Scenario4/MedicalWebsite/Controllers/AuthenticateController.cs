using MedicalWebsite.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.UmaManager.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalWebsite.Controllers
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

        /// <summary>
        /// Authenticate the user and create a cookie.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Callback()
        {
            var idToken = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "id_token");
            if (idToken.Equals(default(KeyValuePair<string, StringValues>)))
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

            claims.Add(new Claim("id_token", idToken.Value));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // Set claims & create the cookie
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authenticationManager = this.GetAuthenticationManager();
            await authenticationManager.SignInAsync(Constants.CookieName,
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

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SimpleIdentityServer.Core.Jwt.Signature;
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

        public AuthenticateController(IJwsParser jwsParser)
        {
            _jwsParser = jwsParser;
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

            var jwsPayload = _jwsParser.GetPayload(identityToken);
            var claims = new List<Claim>();
            foreach (var claim in jwsPayload)
            {
                claims.Add(new Claim(claim.Key, claim.Value.ToString()));
            }

            await SecurityProxyWebApplication.GetRptTokenByRecursion(identityToken.First(), 
                accessToken.First(), 
                accessToken.First());

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authenticationManager = this.GetAuthenticationManager();
            authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
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

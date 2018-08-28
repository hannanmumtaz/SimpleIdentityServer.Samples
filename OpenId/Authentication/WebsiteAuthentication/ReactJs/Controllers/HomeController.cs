using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Authenticate.SMS.Client;
using SimpleIdentityServer.Client;
using System;
using System.Collections.Generic;
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
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretBasicAuth(Constants.ClientId, Constants.ClientSecret)
                .UsePassword(authenticateRequest.Login, authenticateRequest.Password, "openid", "profile", "role")
                .ResolveAsync(Constants.OpenIdWellKnownConfiguration)
                .ConfigureAwait(false);
            if (grantedToken.ContainsError)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(grantedToken.Content);
        }

        [HttpPost]
        public async Task<IActionResult> SendConfirmationCode([FromBody] SmsAuthenticateRequest authenticateRequest)
        {
            Check(authenticateRequest);
            var factory = new SidSmsAuthenticateClientFactory();
            await factory.GetClient().Send(Constants.OpenIdBaseUrl, new SimpleIdentityServer.Authenticate.SMS.Common.Requests.ConfirmationCodeRequest
            {
                PhoneNumber = authenticateRequest.PhoneNumber
            });
            return new NoContentResult();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmConfirmationCode([FromBody] SmsConfirmationRequest confirmationRequest)
        {
            Check(confirmationRequest);
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector().UseClientSecretBasicAuth(Constants.ClientId, Constants.ClientSecret)
                .UsePassword(confirmationRequest.PhoneNumber, confirmationRequest.ConfirmationCode, new List<string> { "sms" }, new List<string> { "openid", "profile", "role" })
                .ResolveAsync(Constants.OpenIdWellKnownConfiguration)
                .ConfigureAwait(false);
            if (grantedToken.ContainsError)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(grantedToken.Content);
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

        private static void Check(SmsAuthenticateRequest smsAuthenticateRequest)
        {
            if (smsAuthenticateRequest == null)
            {
                throw new ArgumentNullException(nameof(smsAuthenticateRequest));
            }

            if (string.IsNullOrWhiteSpace(smsAuthenticateRequest.PhoneNumber))
            {
                throw new ArgumentNullException(nameof(smsAuthenticateRequest.PhoneNumber));
            }
        }

        private static void Check(SmsConfirmationRequest smsConfirmationRequest)
        {
            if (smsConfirmationRequest == null)
            {
                throw new ArgumentNullException(nameof(smsConfirmationRequest));
            }

            if (string.IsNullOrWhiteSpace(smsConfirmationRequest.PhoneNumber))
            {
                throw new ArgumentNullException(nameof(smsConfirmationRequest.PhoneNumber));
            }

            if (string.IsNullOrWhiteSpace(smsConfirmationRequest.ConfirmationCode))
            {
                throw new ArgumentNullException(nameof(smsConfirmationRequest.ConfirmationCode));
            }
        }
    }
}

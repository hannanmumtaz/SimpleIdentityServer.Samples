using MedicalWebsite.Extensions;
using MedicalWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Client.DTOs.Response;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalWebsite.Controllers
{
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GiveAccess(AccessViewModel accessViewModel)
        {
            if (accessViewModel == null)
            {
                throw new ArgumentNullException(nameof(accessViewModel));
            }

            var principal = User as ClaimsPrincipal;
            var rpt = await GetRpt(Constants.WriteScope);

            var identityServerUmaClientFactory = new IdentityServerUmaClientFactory();
            var policyClient = identityServerUmaClientFactory.GetPolicyClient();
            var policy = await policyClient.GetByResolution(rpt.Key.ResourceSet.AuthorizationPolicy, Constants.UmaConfigurationUrl, rpt.Value);
            var rules = policy.Rules.Select(r => new PutPolicyRule
            {
                Id = r.Id,
                Claims = r.Claims,
                ClientIdsAllowed = r.ClientIdsAllowed,
                IsResourceOwnerConsentNeeded = r.IsResourceOwnerConsentNeeded,
                Scopes = r.Scopes,
                Script = r.Script
            }).ToList();
            rules.Add(new PutPolicyRule
            {
                ClientIdsAllowed = new List<string> { Constants.ClientId },
                Id = Guid.NewGuid().ToString(),
                Scopes = new List<string> { Constants.ReadScope },
                IsResourceOwnerConsentNeeded = false,
                Claims = new List<PostClaim>
                {
                    new PostClaim
                    {
                        Type = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject,
                        Value = accessViewModel.Id
                    }
                }
            });
            var putPolicy = new PutPolicy
            {
                PolicyId = policy.Id,
                Rules = rules
            };
            await policyClient.UpdateByResolution(putPolicy, Constants.UmaConfigurationUrl, rpt.Value);
            return RedirectToAction("Index");
        }

        private async Task<KeyValuePair<GetRptResponse, string>> GetRpt(string scope)
        {
            var principal = User as ClaimsPrincipal;
            var idToken = ClaimPrincipalExtensions.GetClaimValue(principal, "id_token");
            var accessToken = await GetAccessToken();
            var resp = await SecurityProxyWebApplication.GetRpt(idToken, accessToken.AccessToken, accessToken.AccessToken, accessToken.AccessToken, scope);
            return new KeyValuePair<GetRptResponse, string>(resp, accessToken.AccessToken);
        }

        private async Task<GrantedToken> GetAccessToken()
        {
            var identityServerClientFactory = new IdentityServerClientFactory();
            return await identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretBasicAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma", "uma_protection", "uma_authorization", "website_api")
                .ResolveAsync(Constants.OpenIdConfigurationUrl);
        }
    }
}

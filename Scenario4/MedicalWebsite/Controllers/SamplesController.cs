#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using MedicalWebsite.Clients;
using MedicalWebsite.Extensions;
using MedicalWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Client.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalWebsite.Controllers
{
    public class SamplesController : Controller
    {
        public SamplesController() { }

        public async Task<IActionResult> Index()
        {
            if (User == null || User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            var principal = User as ClaimsPrincipal;
            var subject = principal.GetSubject();
            var rpt = await GetRpt("read");
            var client = new SamplesClient();
            var result = await client.GetSamples(subject, rpt.Key.Rpt);
            var viewModel = new SampleViewModel
            {
                Id = result.Id
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Display()
        {
            if (User == null || User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            SampleViewModel viewModel = null;
            try
            {
                var principal = User as ClaimsPrincipal;
                var subject = principal.GetSubject();
                var rpt = await GetRpt("read");
                var client = new SamplesClient();
                var result = await client.GetSamples(subject, rpt.Key.Rpt);
                viewModel.Id = result.Id;
            }
            catch(Exception)
            {
                return Content("Access denied");
            }

            return View("Index", viewModel);
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

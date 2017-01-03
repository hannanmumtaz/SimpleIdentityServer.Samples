using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Client.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
#if NET
using System.Net;
#endif
using System.Net.Http;

namespace MarketingClient.Controllers
{
    public struct Rating
    {
        public string ClientName { get; set; }
        public int Value { get; set; }
    }

    [Route("api/ratings")]
    public class RatingsController
    {        
        // NOTE : Store the RPT token into caching store.
        private class RptToken
        {
            public string Value { get; set; }
            public double SlidingExpTime { get; set; }
            public DateTime CreateDateTime { get; set; }
        }

        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly static RptToken _rptToken = new RptToken();

        public RatingsController()
        {
            _identityServerClientFactory = new IdentityServerClientFactory();
        }

        public async Task<List<Rating>> Get()
        {
            string rptToken = _rptToken.Value;
            var expTime = _rptToken.CreateDateTime.AddMilliseconds(_rptToken.SlidingExpTime);
            if (DateTime.UtcNow > expTime)
            {
                GrantedToken token = await GetAccessToken();
                rptToken = await SecurityProxyClientApi.GetRptToken(token.AccessToken, token.AccessToken, token.AccessToken);
                _rptToken.CreateDateTime = DateTime.UtcNow;
                _rptToken.SlidingExpTime = 20000;
                _rptToken.Value = rptToken;
            }

            // Retrieve rpt token
            // Get protected resource and returns ratings
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5102/api/Clients")
            };
            request.Headers.Add("Authorization", "Bearer " + rptToken);
            var result = await httpClient.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<List<string>>(content);
            var res = new List<Rating>();
            foreach(var client in clients)
            {
                res.Add(new Rating
                {
                    ClientName = client,
                    Value = 5 
                });
            }

            return res;
        }

        private async Task<GrantedToken> GetAccessToken()
        {
            return await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretBasicAuth(Constants.ClientId, Constants.ClientSecret)
                .UseClientCredentials("uma_authorization", "uma_protection", "website_api", "uma")
                .ResolveAsync(Constants.OpenidConfigurationUrl);
        }
    }
}

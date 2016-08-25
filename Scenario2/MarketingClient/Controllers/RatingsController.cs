﻿using Microsoft.AspNetCore.Mvc;
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
        #region Fields

        private const string _clientId = "123d900f-1586-4ab2-b5c9-8d05d6e7aaae";

        private const string _clientSecret = "fdc25115-7079-458a-a36f-7e6822c3bad2";

        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        #endregion

        #region Constructor

        public RatingsController()
        {
            _identityServerClientFactory = new IdentityServerClientFactory();
        }

        #endregion

        #region Public methods

        public async Task<List<Rating>> Get()
        {
            // Retrieve rpt token
            GrantedToken token = await GetAccessToken();
            string rptToken = await SecurityProxyClientApi.GetRptToken(token.AccessToken, token.AccessToken);
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

        #endregion

        #region Private methods

        private async Task<GrantedToken> GetAccessToken()
        {
            return await _identityServerClientFactory.CreateTokenClient()
                .UseClientSecretBasicAuth(_clientId, _clientSecret)
                .UseClientCredentials("uma_authorization", "uma_protection")
                .ResolveAsync("https://localhost:5443/.well-known/openid-configuration");
        }

        #endregion
    }
}
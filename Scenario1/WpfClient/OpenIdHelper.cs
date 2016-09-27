using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WpfClient
{
    public class Tokens
    {
        public string AccessToken { get; set; }

        public string IdentityToken { get; set; }
    }
    
    internal static class OpenIdHelper
    {
        #region Public static methods

        public static async Task<string> GetAuthorizationUrl()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Constants.OpenidConfigurationUrl)
            };
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(str);
            var authEndPoint = json.authorization_endpoint;
            return $"{authEndPoint}?scope=openid role profile uma_authorization uma_protection website_api uma&state=75BCNvRlEGHpQRCT&redirect_uri={Constants.RedirectUrl}&response_type=id_token token&client_id={Constants.ClientId}&nonce=nonce&response_mode=fragment";
        }

        public static Tokens GetTokens(string url)
        {
            var fragment = new Uri(url).Fragment.TrimStart('#');
            var queries = HttpUtility.ParseQueryString(fragment);
            return new Tokens
            {
                AccessToken = queries["access_token"],
                IdentityToken = queries["id_token"]
            };
        }

        public static bool IsCallback(string url)
        {
            return url.StartsWith(Constants.RedirectUrl);
        }

        #endregion
    }
}

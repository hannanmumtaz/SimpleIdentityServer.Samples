using System;
using System.Web;

namespace Scenario1.WpfClient
{
    internal class Tokens
    {
        public string AccessToken { get; set; }

        public string IdentityToken { get; set; }
    }
    
    internal static class OpenIdHelper
    {
        #region Fields

        private const string Url = "https://localhost:5443";

        #endregion

        #region Public static methods

        public static string GetAuthorizationUrl()
        {
            return $"{Url}/authorization?scope=openid role profile&state=75BCNvRlEGHpQRCT&redirect_uri={Constants.ClientInfo.RedirectUrl}&response_type=id_token token&client_id={Constants.ClientInfo.ClientId}&nonce=nonce&response_mode=query";
        }

        public static Tokens GetTokens(string url)
        {
            var queries = HttpUtility.ParseQueryString(new Uri(url).Query);
            return new Tokens
            {
                AccessToken = queries["access_token"],
                IdentityToken = queries["id_token"]
            };
        }

        public static bool IsCallback(string url)
        {
            return url.StartsWith(Constants.ClientInfo.RedirectUrl);
        }

        #endregion
    }
}

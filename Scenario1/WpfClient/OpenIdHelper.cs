using System;
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

        public static string GetAuthorizationUrl()
        {
            return $"{Constants.BaseOpenidUrl}/authorization?scope=openid role profile uma_authorization uma_protection website_api&state=75BCNvRlEGHpQRCT&redirect_uri={Constants.RedirectUrl}&response_type=id_token token&client_id={Constants.ClientId}&nonce=nonce&response_mode=query";
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
            return url.StartsWith(Constants.RedirectUrl);
        }

        #endregion
    }
}

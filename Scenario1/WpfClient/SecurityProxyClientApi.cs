using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfClient
{
    public static class SecurityProxyClientApi
    {
        /// <summary>
        /// Get an RPT token to access to a protected resource.
        /// <see cref="https://docs.kantarainitiative.org/uma/rec-uma-core.html#uma-bearer-token-profile" /> for more information about the statement.
        /// </summary>
        /// <param name="idToken">JWS token used by the authorization policy</param>
        /// <param name="umaProtectionToken">Token valid for the scope uma_protection</param>
        /// <param name="umaAuthorizationToken">Token valid for the scope uma_authorization</param>
        /// <param name="resourceToken">Token valid for the scope website_api</param>
        /// <returns>An rpt token</returns>
        public static async Task<string> GetRptToken(
            string idToken,
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                UmaConfigurationUrl = Constants.UmaConfigurationUrl,
                OpenidConfigurationUrl = Constants.OpenidConfigurationUrl,
                RootManageApiUrl = Constants.RootManageApiUrl
            });
            try
            {
                var result = await proxy.GetRpt("resources/Apis/ClientApi/v1/ClientsController/Get", idToken, umaProtectionToken, umaAuthorizationToken, resourceToken, new List <string>
                {
                    "execute"
                });
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

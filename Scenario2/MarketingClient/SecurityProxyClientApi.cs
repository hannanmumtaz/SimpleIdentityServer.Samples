using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketingClient
{
    public static class SecurityProxyClientApi
    {
        private static IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();

        /// <summary>
        /// Get an RPT token to access to a protected resource.
        /// <see cref="https://docs.kantarainitiative.org/uma/rec-uma-core.html#uma-bearer-token-profile" /> for more information about the statement.
        /// </summary>
        /// <param name="umaProtectionToken">Token valid for the scope uma_protection</param>
        /// <param name="umaAuthorizationToken">Token valid for the scope uma_authorization</param>
        /// <param name="resourceToken">Token valid for the scope website_api</param>
        /// <returns>An rpt token</returns>
        public static async Task<string> GetRptToken(
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken)
        {
            var factory = new SecurityProxyFactory();
            var opt = new SecurityOptions
            {
                UmaConfigurationUrl = Constants.UmaConfigurationUrl,
                OpenidConfigurationUrl = Constants.OpenidConfigurationUrl,
                RootManageApiUrl = Constants.RootManageApiUrl
            };
            var proxy = factory.GetProxy(opt);
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            var resources = await resourceClient
                        .SearchResources(new SearchResourceRequest
                        {
                            IsExactUrl = true,
                            AuthorizationPolicyFilter = AuthorizationPolicyFilters.All,
                            Url = "resources/Apis/ClientApi/v1/ClientsController/Get"
                        }, Constants.ResourcesUrl, resourceToken);
            try
            {
                var result = await proxy.GetRpt(resources.First().ResourceSetId, umaProtectionToken, umaAuthorizationToken, new List<string>
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

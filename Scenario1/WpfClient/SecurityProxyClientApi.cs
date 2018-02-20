using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfClient
{
    public static class SecurityProxyClientApi
    {
        private static IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();

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
                var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
                var resources = await resourceClient
                            .SearchResources(new SearchResourceRequest
                            {
                                IsExactUrl = true,
                                AuthorizationPolicyFilter = AuthorizationPolicyFilters.All,
                                Url = "resources/Apis/ClientApi/v1/ClientsController/Get"
                            }, "https://localhost:5444/api/vs/resources", resourceToken);
                var resourceSetId = resources.First().ResourceSetId;
                var result = await proxy.GetRpt(resourceSetId, idToken, umaProtectionToken, umaAuthorizationToken, new List <string>
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

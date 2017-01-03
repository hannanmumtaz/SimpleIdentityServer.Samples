using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class SecurityProxyWebApplication
    {
        private static IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();

        public static async Task<IEnumerable<string>> GetRptTokenByRecursion(
            string idToken,
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                UmaConfigurationUrl = Constants.UmaConfigurationUrl,
                OpenidConfigurationUrl = Constants.OpenIdConfigurationUrl,
                RootManageApiUrl = Constants.RootManagerApiUrl
            });
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            var resources = await resourceClient
                        .SearchResources(new SearchResourceRequest
                        {
                            IsExactUrl = true,
                            AuthorizationPolicyFilter = AuthorizationPolicyFilters.All,
                            Url = Constants.WebApplicationResource
                        }, Constants.ResourcesUrl, resourceToken);
            return await proxy.GetRpts(resources.Select(r => r.ResourceSetId), idToken, umaProtectionToken, umaAuthorizationToken, new List<string>
                    {
                        "execute",
                        "read",
                        "write"
                    });
        }
    }
}

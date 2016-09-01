using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication
{
    public class SecurityProxyWebApplication
    {
        private static IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();

        public static async Task<List<string>> GetRptTokenByRecursion(
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
            var res = new List<string>();
            foreach (var resource in resources)
            {
                try
                {
                    var result = await proxy.GetRpt(resource.Url, idToken, umaProtectionToken, umaAuthorizationToken, resourceToken, new List<string>
                    {
                        "execute",
                        "read",
                        "write"
                    });
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        continue;
                    }

                    res.Add(result);
                }
                catch(Exception ex)
                {
                    continue;
                }
            }

            return res;
        }
    }
}

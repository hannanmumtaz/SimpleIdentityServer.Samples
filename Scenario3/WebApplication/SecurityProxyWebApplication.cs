using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
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
            string umaAuthorizationToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                ClientId = "e8db4d32-dbd1-4b62-b8c0-62f50e539e21",
                ClientSecret = "cdc93990-b5f3-4788-8dd1-50858cf9394f",
                UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration",
                OpenidConfigurationUrl = "https://localhost:5443/.well-known/openid-configuration",
                RootManageApiUrl = "https://localhost:5444/api"
            });
            const string resourceUrl = "resources/WebSite";
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            var resources = await resourceClient
                        .SearchResources(new SearchResourceRequest
                        {
                            IsExactUrl = true,
                            AuthorizationPolicyFilter = AuthorizationPolicyFilters.All,
                            Url = resourceUrl
                        }, "https://localhost:5444/api/vs/resources", string.Empty);
            var res = new List<string>();
            foreach(var resource in resources)
            {
                try
                {
                    var result = await proxy.GetRpt(resource.Url, idToken, umaProtectionToken, umaAuthorizationToken, new List<string>
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
                catch
                {
                    continue;
                }
            }

            return res;
        }
    }
}

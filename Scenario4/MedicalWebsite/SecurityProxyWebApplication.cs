using SimpleIdentityServer.Proxy;
using SimpleIdentityServer.UmaManager.Client;
using SimpleIdentityServer.UmaManager.Client.DTOs.Requests;
using SimpleIdentityServer.UmaManager.Client.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalWebsite
{
    public class GetRptResponse
    {
        public ResourceResponse ResourceSet { get; set; }
        public string Rpt { get; set; }
    }

    public class SecurityProxyWebApplication
    {
        private static IIdentityServerUmaManagerClientFactory _identityServerUmaManagerClientFactory = new IdentityServerUmaManagerClientFactory();

        public static async Task<GetRptResponse> GetRpt(
            string idToken,
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken,
            string scope)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                UmaConfigurationUrl = Constants.UmaConfigurationUrl,
                OpenidConfigurationUrl = Constants.OpenIdConfigurationUrl,
                RootManageApiUrl = Constants.RootManagerApiUrl
            });
            var resourceClient = _identityServerUmaManagerClientFactory.GetResourceClient();
            var resource = (await resourceClient.SearchResources(new SearchResourceRequest
            {
                IsExactUrl = true,
                Url = Constants.ApplicationResourceUrl,
            }, Constants.ResourcesUrl, resourceToken)).First();
            var rpt = await proxy.GetRpt(resource.ResourceSetId, idToken, umaProtectionToken, umaAuthorizationToken, new List<string>
                    {
                        scope
                    });
            return new GetRptResponse
            {
                ResourceSet = resource,
                Rpt = rpt
            };
        }
    }
}

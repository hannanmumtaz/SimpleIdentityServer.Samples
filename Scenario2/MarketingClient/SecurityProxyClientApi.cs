using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketingClient
{
    public static class SecurityProxyClientApi
    {
        public static async Task<string> GetRptToken(
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken)
        {
            var factory = new SecurityProxyFactory();
            var opt = new SecurityOptions
            {
                UmaConfigurationUrl = "https://lokit.westus.cloudapp.azure.com:5445/.well-known/uma-configuration",
                OpenidConfigurationUrl = "https://lokit.westus.cloudapp.azure.com:5443/.well-known/openid-configuration",
                RootManageApiUrl = "https://lokit.westus.cloudapp.azure.com:5444/api"
            };
            var proxy = factory.GetProxy(opt);
            try
            {
                var result = await proxy.GetRpt("resources/Apis/ClientApi/v1/ClientsController/Get", umaProtectionToken, umaAuthorizationToken, resourceToken, new List<string>
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

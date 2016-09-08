using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfClient
{
    public static class SecurityProxyClientApi
    {
        public static async Task<string> GetRptToken(
            string idToken,
            string umaProtectionToken,
            string umaAuthorizationToken,
            string resourceToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                UmaConfigurationUrl = "https://lokit.westus.cloudapp.azure.com:5445/.well-known/uma-configuration",
                OpenidConfigurationUrl = "https://lokit.westus.cloudapp.azure.com:5443/.well-known/openid-configuration",
                RootManageApiUrl = "https://lokit.westus.cloudapp.azure.com:5444/api"
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

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
            string umaAuthorizationToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                ClientId = Constants.ClientInfo.ClientId,
                ClientSecret = Constants.ClientInfo.ClientSecret,
                UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration",
                OpenidConfigurationUrl = "https://localhost:5443/.well-known/openid-configuration",
                RootManageApiUrl = "https://localhost:5444/api"
            });
            try
            {
                var result = await proxy.GetRpt("resources/Apis/ClientApi/v1/ClientsController/Get", idToken, umaProtectionToken, umaAuthorizationToken, new List<string>
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

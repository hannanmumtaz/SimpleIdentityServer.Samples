using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scenario1.WpfClient
{
    public static class SecurityProxyClientApi
    {
        public static async Task<string> GetRptToken(string idToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                ClientId = Constants.ClientInfo.ClientId,
                ClientSecret = Constants.ClientInfo.ClientSecret,
                UmaConfigurationUrl = "http://localhost:5001/.well-known/uma-configuration",
                OpenidConfigurationUrl = "http://localhost:5000/.well-known/openid-configuration",
                RootManageApiUrl = "https://localhost:5444/api"
            });
            try
            {
                var result = await proxy.GetRpt("resources/Apis/ClientApi/v1/ClientsController/Get", idToken, new List<string>
                {
                    "execute",
                    "write",
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

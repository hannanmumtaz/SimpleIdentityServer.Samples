using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;

namespace Scenario1.WpfClient
{
    public static class SecurityProxyClientApi
    {
        public static string GetRptToken(string idToken)
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                ClientId = "<replace by your client id>",
                ClientSecret = "<replace by your client secret>",
                UmaConfigurationUrl = "http://localhost:5001/.well-known/uma-configuration",
                OpenidConfigurationUrl = "http://localhost:5000/.well-known/openid-configuration",
                RootManageApiUrl = "http://localhost:8080/api"
            });
            try
            {
                var result = proxy.GetRpt("{resource_url}", idToken, new List<string>
                {
                    "execute",
                    "write",
                    "execute"
                }).Result;
                return result;
            }
            catch (AggregateException)
            {
                return null;
            }
        }
    }
}

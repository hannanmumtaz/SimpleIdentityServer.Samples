#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;

namespace Scenario1.WpfClient
{
    public class SecurityProxy_577624e8c1a8a64c44c73537
    {
        public static string GetRptToken()
        {
            var factory = new SecurityProxyFactory();
            var proxy = factory.GetProxy(new SecurityOptions
            {
                ClientId = "<replace by your client id>",
                ClientSecret = "<replace by your client secret>",
                UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration",
                OpenidConfigurationUrl = "https://localhost:5443/.well-known/openid-configuration",
                RootManageApiUrl = "https://localhost:5444/api"
            });
            try
            {
                var result = proxy.GetRpt("resources/Apis/TestApiProject", new List<string>
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
                var result = proxy.GetRpt("resources/Apis/TestApiProject", idToken, new List<string>
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

using Newtonsoft.Json;
using SimpleIdentityServer.Uma.EF;
using SimpleIdentityServer.Uma.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteProtection.UmaProvider.Extensions
{
    public static class SimpleIdServerUmaContextExtensions
    {
        private static string _firstPolicyId = "986ea7da-d911-48b8-adfa-124b3827246a";

        #region Public static methods

        public static void EnsureSeedData(this SimpleIdServerUmaContext context)
        {
            InsertResources(context);
            InsertPolicies(context);
            try
            {
                context.SaveChanges();
            }
            catch { }
        }

        #endregion

        #region Private static methods

        private static void InsertResources(SimpleIdServerUmaContext context)
        {
            if (!context.ResourceSets.Any())
            {
                context.ResourceSets.AddRange(new[]
                {
                    new ResourceSet
                    {
                        Id = "1",
                        Name = "Access to administrator",
                        Scopes = "read,write,execute"
                    }
                });
            }
        }

        private static void InsertPolicies(SimpleIdServerUmaContext context)
        {
            if (!context.Policies.Any())
            {
                var claims = new List<SimpleIdentityServer.Uma.Core.Models.Claim>();
                claims.Add(new SimpleIdentityServer.Uma.Core.Models.Claim { Type = "sub", Value = "administrator" });
                context.Policies.AddRange(new[]
                {
                    new Policy
                    {
                        Id = _firstPolicyId,
                        PolicyResources = new List<PolicyResource>
                        {
                            new PolicyResource
                            {
                                PolicyId = _firstPolicyId,
                                ResourceSetId = "1"
                            }
                        },
                        Rules = new List<PolicyRule>
                        {
                            new PolicyRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                PolicyId = _firstPolicyId,
                                IsResourceOwnerConsentNeeded = false,
                                ClientIdsAllowed = "",
                                Scopes = "read,write,execute",
                                Claims = JsonConvert.SerializeObject(claims),
                                OpenIdProvider = "http://localhost:60000/.well-known/openid-configuration"
                            }
                        }
                    }
                });
            }
        }

        #endregion
    }
}
